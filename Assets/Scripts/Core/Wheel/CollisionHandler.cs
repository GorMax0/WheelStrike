using System;
using UnityEngine;
using UnityEngine.Events;
using Empty;
using Services.GameStates;

namespace Core.Wheel
{
    public class CollisionHandler : MonoBehaviour
    {
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event UnityAction CollidedWithGround;
        public event UnityAction<int, float> CollidedWithObstacle;
        public event UnityAction<int> CollidedWithCar;

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
            switch (state)
            {
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameFinished()
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
                CollidedWithObstacle?.Invoke(obstacle.Reward, obstacle.HalfMass);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
                return;

            OnCollisionGround(collision);
            OnCollisionObstacle(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Car car))
            {
                car.Explode();
                car.StopMove();
                CollidedWithCar?.Invoke(car.Reward);
            }

            if (other.TryGetComponent(out Wall wall))
            {
                wall.EnableGravityBricks();
            }

            if (other.TryGetComponent(out CameraTrigger cameraTrigger))
            {
                cameraTrigger.OnTriggerEnterWheel();
            }
        }
    }
}