using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;

        private Rigidbody _rigidbody;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody>();

        public void Rotate(Vector3 direction, float deltaTime)
        {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, deltaTime * _speed);
        }
    }
}