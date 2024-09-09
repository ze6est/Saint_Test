using SaintTest.CodeBase.Inputs;
using UnityEngine;
using Zenject;

namespace SaintTest.CodeBase.Players
{
    public class PlayerController : ITickable, IFixedTickable
    {
        private readonly InputHandler _inputHandler;
        private readonly PlayerMover _playerMover;

        private readonly PlayerRotator _playerRotator;

        private Vector3 _moveDirection;

        public PlayerController(InputHandler inputHandler, PlayerMover playerMover, PlayerRotator playerRotator)
        {
            _inputHandler = inputHandler;
            _playerMover = playerMover;
            _playerRotator = playerRotator;
        }
        
        public void Tick() => 
            _moveDirection = _inputHandler.Direction;

        public void FixedTick()
        {
            if (_moveDirection != Vector3.zero)
                _playerMover.Move(_moveDirection, Time.fixedDeltaTime);

            if (_moveDirection.magnitude > 0.1f)
                _playerRotator.Rotate(_moveDirection, Time.fixedDeltaTime);
        }
    }
}