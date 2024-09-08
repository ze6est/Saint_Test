using SaintTest.CodeBase.Inputs;
using UnityEngine;
using Zenject;

namespace SaintTest.CodeBase.Players
{
    public class PlayerController : IFixedTickable
    {
        private readonly InputHandler _inputHandler;
        private readonly PlayerMover _playerMover;

        private readonly PlayerRotator _playerRotator;

        public PlayerController(InputHandler inputHandler, PlayerMover playerMover, PlayerRotator playerRotator)
        {
            _inputHandler = inputHandler;
            _playerMover = playerMover;
            _playerRotator = playerRotator;
        }

        public void FixedTick()
        {
            Vector3 moveDirection = _inputHandler.Direction;

            if (moveDirection != Vector3.zero)
                _playerMover.Move(moveDirection, Time.fixedDeltaTime);

            if (moveDirection.magnitude > 0.1f)
                _playerRotator.Rotate(moveDirection, Time.fixedDeltaTime);
        }
    }
}