using System;
using UnityEngine;
using UnityEngine.Events;
using Services.GameStates;

namespace Core.Wheel
{
    public class CollisionHandler : MonoBehaviour
    {
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event UnityAction CollidedWithGround;
        public event UnityAction<int> CollidedWithObstacle;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateService;
        }


        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{typeof(CollisionHandler)}: Initialize(GameStateService gameStateService) : Already initialized.");

            _gameStateService = gameStateService;
            OnEnable();

            _isInitialized = true;
        }

        private void OnGameStateService(GameState state)
        {
            switch(state)
            {
                case GameState.Failed:
                    OnGameFailed();
                    break;
            }
        }

        private void OnGameFailed()
        {
            enabled = false;
        }

        private void OnCollisionGround(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Ground ground))
            {
                CollidedWithGround?.Invoke();
            }
        }

        private void OnCollisionObstacle(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Obstacle obstacle))
            {
                CollidedWithObstacle?.Invoke(obstacle.Reward);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
                return;

            OnCollisionGround(collision);
            OnCollisionObstacle(collision);
        }
    }
}