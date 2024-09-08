using System.Collections.Generic;
using SaintTest.CodeBase.Factories;
using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase.Pool
{
    public class ItemPool
    {
        private readonly List<Item> _items;
        private readonly ItemFactory _factory;
        private readonly Transform _container;

        public ItemPool(List<Item> prefabs, int count, ItemFactory factory, Transform container)
        {
            _items = new();
            _factory = factory;
            _container = container;
            
            foreach (Item item in prefabs)
            {
                for (int i = 0; i < count; i++)
                {
                    Release(CreateItem(item));
                }
            }
        }
        
        public Item Get(Item item)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].gameObject.activeInHierarchy && _items[i].ItemData == item.ItemData)
                {
                    Item current = _items[i];
                    Activate(current);
                    
                    return current;
                }
            }

            Item newItem = CreateItem(item);
            Activate(newItem);
            
            return newItem;
        }
        
        public void Release(Item item) => 
            item.gameObject.SetActive(false);

        private Item CreateItem(Item item)
        {
            Item newItem = _factory.Create(item, _container);
            
            _items.Add(newItem);

            return newItem;
        }

        private void Activate(Item item) => 
            item.gameObject.SetActive(true);
    }
}