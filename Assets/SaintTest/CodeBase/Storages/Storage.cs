using System.Collections.Generic;
using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase.Storages
{
    public class Storage : MonoBehaviour, IStorage
    {
        [SerializeField] private Item _item;
        [SerializeField] private GameObject _model;
        [SerializeField] private int _maxCapacity;
        [SerializeField] private Transform _itemsPoint;

        private Stack<Item> _items;

        [field: SerializeField] public StorageType Type { get; private set; }

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
            _items = new Stack<Item>();
        }

        public Item Send()
        {
            _itemsPoint.position -= new Vector3(0, _item.Height, 0);
            Item item = _items.Pop();
            item.transform.parent = null;
            return item;
        }

        public void Take(Item item)
        {
            _itemsPoint.position += new Vector3(0, _item.Height, 0);
            item.transform.rotation = transform.rotation;
            item.transform.parent = transform;
            _items.Push(item);
        }

        public bool HasItem(Item item) => 
            _item.Type == item.Type;
    }
}