using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Storages;
using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerStorage : MonoBehaviour, ISender, ITaker
    {
        [SerializeField] private int _maxCapacity;
        [SerializeField] private Transform _itemsPoint;

        private Stack<Item> _items;

        private Storage _currentStorage;
        
        private bool _inTrigger;
        private bool _isRunning;
        
        private CancellationTokenSource _runToken;
        private CancellationTokenSource _transitionToken;

        private void Awake()
        {
            _items = new Stack<Item>();
            
            _transitionToken = new CancellationTokenSource();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Storage storage))
            {
                _currentStorage = storage;
                _inTrigger = true;
                
                if (!_isRunning)
                {
                    _isRunning = true;
                    _runToken = new CancellationTokenSource();
                    Run(_runToken.Token).Forget();
                }
            }
        }
        
        private void OnTriggerExit(Collider other) => 
            _inTrigger = false;

        private void OnDestroy()
        {
            _runToken?.Cancel();
            _transitionToken.Cancel();
        }

        private async UniTask Run(CancellationToken cancellationToken)
        {
            while (_inTrigger)
            {
                if (_currentStorage.Type == StorageType.ToProduce)
                {
                    if (!_currentStorage.IsEmpty && _items.Count < _maxCapacity)
                    {
                        Transition toPlayer = new Transition(_currentStorage, this, _itemsPoint);
                        await toPlayer.Run(_transitionToken.Token);
                    }
                }
                else if (_currentStorage.Type == StorageType.ToConsume)
                {
                    if (_items.Count > 0 && _currentStorage.HasItem(_items.Peek()) && !_currentStorage.IsFull)
                    {
                        Transition toStorage = new Transition(this, _currentStorage, _currentStorage.NextItemPosition);
                        await toStorage.Run(_transitionToken.Token);
                    }
                }
                
                await UniTask.Yield(cancellationToken);
            }
            
            _runToken?.Cancel();
            _isRunning = false;
        }

        public Item Send()
        {
            Item item = _items.Pop();
            item.transform.parent = null;
            _itemsPoint.position -= new Vector3(0, item.Height, 0);
            return item;
        }

        public void Take(Item item)
        {
            _itemsPoint.position += new Vector3(0, item.Height, 0);
            item.transform.parent = transform;
            item.transform.rotation = transform.rotation;
            _items.Push(item);
        }
    }
}