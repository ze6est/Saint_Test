using SaintTest.CodeBase.Items;
using UnityEngine;

namespace SaintTest.CodeBase.Factories
{
    public class ItemFactory
    {
        public Item Create(Item prefab, Transform container)
        {
            Item item = Object.Instantiate(prefab, container);
            return item;
        }
    }
}