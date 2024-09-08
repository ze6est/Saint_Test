using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Logic;
using SaintTest.CodeBase.Storages;
using SaintTest.CodeBase.Transitions;
using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerStorage : MonoBehaviour, ISender, ITaker
    {
        [SerializeField] private Transform _itemsPoint;
        [Space]
        [SerializeField] private int _maxCapacity;

        private Transform _transform;
        
        private Stack<Item> _items;

        private Storage _currentStorage;
        
        private bool _isRunning;
        
        private UniTask _runTask;
        private CancellationTokenSource _runToken;
        private CancellationTokenSource _transitionToken;

        private void Awake()
        {
            _transform = transform;
            _items = new Stack<Item>();
            
            _transitionToken = new CancellationTokenSource();
            _runToken = new CancellationTokenSource();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out Storage storage))
            {
                _currentStorage = storage;
                
                if (!_isRunning)
                {
                    _isRunning = true;
                    _runToken = new CancellationTokenSource();
                    Run(_runToken.Token).Forget();
                }
            }
        }
        
        private void OnTriggerExit(Collider other) => 
            _currentStorage = null;

        private void OnDestroy()
        {
            _runToken?.Cancel();
            _transitionToken?.Cancel();
        }
        
        public Item Send()
        {
            Item item = _items.Pop();
            _itemsPoint.position -= new Vector3(0, item.ItemData.Height, 0);
            return item;
        }

        public void Take(Item item)
        {
            Transform itemTransform = item.transform;
            
            _itemsPoint.position += new Vector3(0, item.ItemData.Height, 0);
            
            itemTransform.parent = _transform;
            itemTransform.rotation = _transform.rotation;
            
            _items.Push(item);
        }

        private async UniTask Run(CancellationToken token)
        {
            while (_currentStorage != null)
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
                
                await UniTask.Yield(token);
            }
            
            _isRunning = false;
            _runToken?.Cancel();
        }
    }
}