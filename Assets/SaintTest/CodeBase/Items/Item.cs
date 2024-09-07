using UnityEngine;

namespace SaintTest.CodeBase.Items
{
    public class Item : MonoBehaviour
    {
        private Vector3 _startPoint;
        
        [field:SerializeField] public ItemType Type { get; private set; }
        [field:SerializeField] public float Height { get; private set; }

        private void Awake() => 
            _startPoint = transform.position;

        public void SetToStartPoint() => 
            transform.position = _startPoint;
    }
}