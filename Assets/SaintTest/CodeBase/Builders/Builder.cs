using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Storages;
using UnityEngine;

namespace SaintTest.CodeBase.Builders
{
    public class Builder : MonoBehaviour, ITaker, ISender
    {
        [SerializeField] private Item _item;
        [SerializeField] private GameObject _model;
        [SerializeField] private Transform _itemsPoint;
        [SerializeField] private Storage _storageToProduce;
        [SerializeField] private Storage[] _storagesToConsume;
        [SerializeField] private float _createTime;
        [SerializeField] private int _poolCapacity;
        [SerializeField] private bool _isPoolAutoExpand;

        private Item _newItem;

        private ObjectsPool<Item> _items;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private void OnValidate()
        {
            if(_item != null && _model != null)
                _model.GetComponent<MeshRenderer>().sharedMaterial
                    = _item.GetComponent<MeshRenderer>().sharedMaterial;
        }

        private void Awake()
        {
            _items = new ObjectsPool<Item>(_item, transform, _isPoolAutoExpand, _poolCapacity);
        }

        private void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Run(_cancellationTokenSource.Token).Forget();
        }
        
        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
        }
        
        public Item Send() => 
            _newItem;

        public void Take(Item item)
        {
            _items.Release(item);
            item.SetToStartPoint();
        }

        private async UniTaskVoid Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                await UniTask.Yield();
                
                if (_storageToProduce.IsFull)
                {
                    continue;
                }

                if (_storagesToConsume.Length > 0)
                {
                    if(_storagesToConsume.Any(storage => storage.IsEmpty))
                        continue;
                }

                if (_storagesToConsume.Length > 0)
                {
                    foreach (Storage storage in _storagesToConsume)
                    {
                        Transition toBuilder = new Transition(storage, this, _itemsPoint);

                        await toBuilder.Run(cancellationToken);
                    }
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(_createTime), false, PlayerLoopTiming.Update, cancellationToken);
                
                _newItem = _items.GetFreeObject();
                
                Transition toStorage = new Transition(this, _storageToProduce, _storageToProduce.NextItemPosition);

                await toStorage.Run(cancellationToken);
            }
        }
    }
}