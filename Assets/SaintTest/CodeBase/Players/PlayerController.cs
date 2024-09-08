using SaintTest.CodeBase.Inputs;
using UnityEngine;

namespace SaintTest.CodeBase.Players
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private PlayerRotator _playerRotator;
        
        private InputHandler _inputHandler;

        private void Awake() => 
            _inputHandler = new InputHandler();

        private void FixedUpdate()
        {
            Vector3 moveDirection = _inputHandler.Direction;

            if (moveDirection != Vector3.zero)
                _playerMover.Move(moveDirection, Time.fixedDeltaTime);

            if (moveDirection.magnitude > 0.1f)
                _playerRotator.Rotate(moveDirection, Time.fixedDeltaTime);
        }
    }
}