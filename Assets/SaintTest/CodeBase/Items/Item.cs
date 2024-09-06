using UnityEngine;

namespace SaintTest.CodeBase.Items
{
    public class Item : MonoBehaviour
    {
        [field:SerializeField] public ItemType Type { get; private set; }
        [field:SerializeField] public float Height { get; private set; }
    }
}