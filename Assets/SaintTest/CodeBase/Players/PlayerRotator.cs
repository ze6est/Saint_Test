using SaintTest.CodeBase.Inputs;
using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerRotator : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        
        private Rigidbody _rigidbody;
        private InputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = new InputHandler();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void FixedUpdate()
        {
            if (_inputHandler.Direction.magnitude < 0.1f)
                return;
                
            float angle = Mathf.Atan2(_inputHandler.Direction.x, _inputHandler.Direction.z) * Mathf.Rad2Deg;
                
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.fixedDeltaTime * _speed);
        }
    }
}