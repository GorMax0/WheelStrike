using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(AnimationWheel))]
    [RequireComponent(typeof(InteractionHandler))]
    public class Player : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Movement _movement;
        private AnimationWheel _animation;
        private InteractionHandler _collisionHandler;
        private Parameter _size;

        public ITravelable Travelable => _movement;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<Movement>();
            _animation = GetComponent<AnimationWheel>();
            _collisionHandler = GetComponent<InteractionHandler>();
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speed, Parameter size)
        {
            _movement.Initialize(gameStateService, coroutineService, aimDirection, speed);
            _animation.Initialize(gameStateService, coroutineService);
            _collisionHandler.Initialize(gameStateService);
            _size = size;
        }
    }
}