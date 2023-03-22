using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Parameters;
using Services.Coroutines;
using Services.GameStates;
using Boost;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(AnimationWheel))]
    [RequireComponent(typeof(InteractionHandler))]
    public class Player : MonoBehaviour
    {
        private const int MassCorrector = 10000;
        private const float MaximumSize = 2.5f;

        [SerializeField] private GameObject _blur;

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

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speed, Parameter size, BoostParameter boost)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speed, Parameter size).");

            _movement.Initialize(gameStateService, coroutineService, aimDirection, speed, size, boost);
            _animation.Initialize(gameStateService, coroutineService);
            _collisionHandler.Initialize(gameStateService);
            _gameStateService = gameStateService;
            _size = size;

            _isInitialized = true;
            OnEnable();
        }

        private void SetSize()
        {
            float newScale = transform.localScale.x + _size.Value > MaximumSize ? MaximumSize : transform.localScale.x + _size.Value;

            transform.localScale = new Vector3(newScale, newScale, newScale);
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
            _blur.SetActive(true);
        }
    }
}