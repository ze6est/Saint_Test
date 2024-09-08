using System.Collections.Generic;
using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase.Storages
{
    public class Storage : MonoBehaviour, IStorage
    {
        [SerializeField] private GameObject _model;
        [Header("Item settings")]
        [SerializeField] private Transform _itemsPoint;
        [SerializeField] private Item _item;
        [Space]
        [SerializeField] private int _maxCapacity;
        
        private Transform _transform;
        
        private Stack<Item> _items;
        
        [field: SerializeField] public StorageType Type { get; private set; }

        public ItemData Item => 
            _item.ItemData;

        public bool IsFull =>
            _items.Count >= _maxCapacity;

        public bool IsEmpty =>
            _items.Count <= 0;

        public Transform NextItemPosition =>
            _itemsPoint;

        private void OnValidate()
        {
            if (_item != null && _model != null)
                _model.GetComponent<MeshRenderer>().sharedMaterial
                    = _item.GetComponent<MeshRenderer>().sharedMaterial;
        }

        private void Awake()
        {
            _transform = transform;
            _items = new Stack<Item>();
        }

        public Item Send()
        {
            _itemsPoint.position -= new Vector3(0, _item.ItemData.Height, 0);
            Item item = _items.Pop();
            return item;
        }

        public void Take(Item item)
        {
            Transform itemTransform = item.transform;
            
            _itemsPoint.position += new Vector3(0, _item.ItemData.Height, 0);
            itemTransform.rotation = _transform.rotation;
            itemTransform.parent = _transform;
            _items.Push(item);
        }

        public bool HasItem(Item item) => 
            _item.ItemData == item.ItemData;
    }
}