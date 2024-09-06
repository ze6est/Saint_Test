using SaintTest.CodeBase.Inputs;
using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;
        
        private Rigidbody _rigidbody;
        private InputHandler _inputHandler;

        private void Awake()
        {
            _inputHandler = new InputHandler();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_inputHandler.Direction == Vector3.zero)
                return;
            
            _rigidbody.MovePosition(transform.position + _inputHandler.Direction * (_speed * Time.fixedDeltaTime));
        }
    }
}