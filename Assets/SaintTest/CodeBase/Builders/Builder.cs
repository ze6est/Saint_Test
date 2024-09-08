using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using SaintTest.CodeBase.Items;
using SaintTest.CodeBase.Logic;
using SaintTest.CodeBase.Pool;
using SaintTest.CodeBase.Storages;
using SaintTest.CodeBase.Transitions;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace SaintTest.CodeBase.Builders
{
    public class Builder : MonoBehaviour, ITaker, ISender
    {
        [SerializeField] private GameObject _model;
        
        [Space] 
        [SerializeField] private float _createTime;

        [Header("Item settings")]
        [SerializeField] private Item _itemPrefab;

        [SerializeField] private Transform _itemsPosition;

        [Header("Storages settings")] [SerializeField]
        private Storage _storageToProduce;

        [SerializeField] private Storage[] _storagesToConsume;

        private ItemPool _itemsPool;
        private Item _newItem;

        private CancellationTokenSource _runToken;

        public event UnityAction<ItemData> StorageFulled;
        public event UnityAction<ItemData, ItemData> StorageEmpted;

        private void OnValidate()
        {
            if (_itemPrefab != null && _model != null)
                _model.GetComponent<MeshRenderer>().sharedMaterial
                    = _itemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
        }

        [Inject]
        public void Construct(ItemPool itemsPool) =>
            _itemsPool = itemsPool;

        private void Awake() =>
            _runToken = new CancellationTokenSource();

        private void Start() =>
            Run(_runToken.Token).Forget();

        private void OnDestroy() =>
            _runToken?.Cancel();

        public Item Send() =>
            _newItem;

        public void Take(Item item)
        {
            item.transform.parent = transform;
            _itemsPool.Release(item);
        }

        private async UniTaskVoid Run(CancellationToken token)
        {
            while (true)
            {
                await UniTask.Yield();

                if (_storageToProduce.IsFull)
                {
                    StorageFulled?.Invoke(_storageToProduce.Item);
                    continue;
                }

                if (_storagesToConsume.Length > 0)
                {
                    if (_storagesToConsume.Any(storage => storage.IsEmpty))
                    {
                        foreach (Storage storage in _storagesToConsume)
                        {
                            if (storage.IsEmpty)
                                StorageEmpted?.Invoke(_itemPrefab.ItemData, storage.Item);
                        }

                        continue;
                    }
                }

                if (_storagesToConsume.Length > 0)
                {
                    foreach (Storage storage in _storagesToConsume)
                    {
                        Transition toBuilder = new Transition(storage, this, _itemsPosition);

                        await toBuilder.Run(token);
                    }
                }

                await UniTask.Delay(TimeSpan.FromSeconds(_createTime), false, PlayerLoopTiming.Update, token);

                _newItem = _itemsPool.Get(_itemPrefab);
                _newItem.transform.position = _itemsPosition.position;

                Transition toStorage = new Transition(this, _storageToProduce, _storageToProduce.NextItemPosition);

                await toStorage.Run(token);
            }
        }
    }
}