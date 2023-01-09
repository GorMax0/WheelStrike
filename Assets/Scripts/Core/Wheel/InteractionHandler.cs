using System;
using UnityEngine;
using UnityEngine.Events;
using Empty;
using Services.GameStates;

namespace Core.Wheel
{
    public class InteractionHandler : MonoBehaviour
    {
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event UnityAction CollidedWithGround;
        public event UnityAction<Obstacle> CollidedWithObstacle;
        public event UnityAction<Car> TriggeredEnterWithCar;
        public event UnityAction<Wall> TriggeredWithWall;
        public event UnityAction<CameraTrigger> TriggeredWithCameraTrigger;

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

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService) : Already initialized.");

            _gameStateService = gameStateService;

            _isInitialized = true;
            OnEnable();
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
                CollidedWithObstacle?.Invoke(obstacle);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
                return;

            OnCollisionGround(collision);
            OnCollisionObstacle(collision);
        }

        private void OnTriggerEnterCar(Collider other)
        {
            if (other.TryGetComponent(out Car car))
            {
                TriggeredEnterWithCar?.Invoke(car);
            }
        }

        private void OnTriggerEnterWall(Collider other)
        {
            if (other.TryGetComponent(out Wall wall))
            {
                TriggeredWithWall?.Invoke(wall);
            }
        }

        private void OnTriggerEnterCameraTrigger(Collider other)
        {
            if (other.TryGetComponent(out CameraTrigger cameraTrigger))
            {
                TriggeredWithCameraTrigger?.Invoke(cameraTrigger);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enabled == false)
                return;

            OnTriggerEnterCar(other);
            OnTriggerEnterWall(other);
            OnTriggerEnterCameraTrigger(other);
        }
    }
}