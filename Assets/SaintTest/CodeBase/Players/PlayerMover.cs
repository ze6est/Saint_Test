using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        
        private Rigidbody _rigidbody;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody>();

        public void Move(Vector3 direction, float deltaTime) => 
            _rigidbody.MovePosition(transform.position + direction * (_speed * deltaTime));
    }
}