using System.Collections.Generic;
using SaintTest.CodeBase.Factories;
using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase.Pool
{
    public class ItemPool : MonoBehaviour
    {
        [SerializeField] private List<Item> _items;
        [SerializeField] private int _count;

        private ItemFactory _factory;
        
        private void Awake()
        {
            _items = new();
            _factory = new ItemFactory();

            foreach (Item item in _items)
            {
                for (int i = 0; i < _count; i++)
                {
                    Release(item);
                    _items.Add(CreateItem(item));
                }
            }
        }
        
        public Item Get(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].gameObject.activeInHierarchy && _items[i].ItemData == item.ItemData)
                {
                    _items[i].gameObject.SetActive(true); 
                    return _items[i];
                }
            }

            return CreateItem(item);
        }
        
        public void Return(Item item)
        {
            item.gameObject.SetActive(false);
            
            _items.Add(item);
        }

        private Item CreateItem(Item item) => 
            _factory.Create(item, transform);

        private void Release(Item item) => 
            item.gameObject.SetActive(false);
    }
}