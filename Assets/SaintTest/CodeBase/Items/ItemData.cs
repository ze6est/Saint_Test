using UnityEngine;

namespace SaintTest.CodeBase.Items
{
    [CreateAssetMenu(menuName = "Create Item", fileName = "Item", order = 51)]
    public class ItemData : ScriptableObject
    {
        [field:SerializeField] public float Height { get; private set; }
    }
}