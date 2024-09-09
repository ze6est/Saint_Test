using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SaintTest.CodeBase.Inputs
{
    public class InputHandler : IDisposable
    {
        public Vector3 Direction { get; private set; }

        private readonly InputControls _inputControls;
        private readonly InputAction _moveAction;

        public InputHandler()
        {
            _inputControls = new InputControls();
            _moveAction = _inputControls.Player.Move;

            _moveAction.Enable();

            _moveAction.performed += MovePerformed;
            _moveAction.canceled += MoveCanceled;
        }

        private void MovePerformed(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            Direction = new Vector3(direction.x, 0, direction.y);
        }

        private void MoveCanceled(InputAction.CallbackContext context) =>
            Direction = Vector3.zero;

        public void Dispose()
        {
            _moveAction.performed -= MovePerformed;
            _moveAction.canceled -= MoveCanceled;
            _moveAction.Disable();
        }
    }
}