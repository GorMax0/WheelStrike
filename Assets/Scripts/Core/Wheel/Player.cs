using System;
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
        private const int MassCorrector = 10000;

        private Rigidbody _rigidbody;
        private Movement _movement;
        private AnimationWheel _animation;
        private InteractionHandler _collisionHandler;
        private Parameter _size;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public ITravelable Travelable => _movement;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<Movement>();
            _animation = GetComponent<AnimationWheel>();
            _collisionHandler = GetComponent<InteractionHandler>();
        }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateService;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speed, Parameter size)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speed, Parameter size).");

            _movement.Initialize(gameStateService, coroutineService, aimDirection, speed, size);
            _animation.Initialize(gameStateService, coroutineService);
            _collisionHandler.Initialize(gameStateService);
            _gameStateService = gameStateService;
            _size = size;

            _isInitialized = true;
            OnEnable();
        }

        private void SetSize()
        {
            float newScaleX = transform.localScale.x + _size.Value;
            float newScaleY = transform.localScale.y + _size.Value;
            float newScaleZ = transform.localScale.z + _size.Value;

            transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);
        }

        private void SetMass() => _rigidbody.mass += _size.Value * MassCorrector;

        private void OnGameStateService(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGameRunning()
        {
            SetSize();
            SetMass();
        }
    }
}