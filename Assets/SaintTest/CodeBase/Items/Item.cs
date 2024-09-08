using UnityEngine;

namespace SaintTest.CodeBase.Items
{
    public class Item : MonoBehaviour
    {
        [field:SerializeField] public ItemData ItemData { get; private set; }
    }
}