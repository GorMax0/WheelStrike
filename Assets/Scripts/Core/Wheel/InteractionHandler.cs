using System;
using Core.Cameras;
using Core.Wall;
using Empty;
using Services.GameStates;
using UnityEngine;

namespace Core.Wheel
{
    public class InteractionHandler : MonoBehaviour
    {
        private GameStateService _gameStateService;
        private bool _isInitialized;

        public event Action CollidedWithGround;

        public event Action<Obstacle> CollidedWithObstacle;

        public event Action<Car.Car> TriggeredEnterWithCar;

        public event Action<Wall.Wall> TriggeredWithWall;

        public event Action<Brick> TriggeredWithBrick;

        public event Action<CameraTrigger> TriggeredWithCameraTrigger;

        public event Action TriggeredNextTile;

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

        private void OnCollisionEnter(Collision collision)
        {
            if (enabled == false)
                return;

            OnCollisionGround(collision);
            OnCollisionObstacle(collision);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (enabled == false)
                return;

            OnTriggerEnterCar(other);
            OnTriggerEnterBirck(other);
            OnTriggerEnterWall(other);
            OnTriggerEnterCameraTrigger(other);
            OnTriggerEnterNextTile(other);
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized)
                throw new InvalidOperationException(
                    $"{GetType()}: Initialize(GameStateService gameStateService) : Already initialized.");

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

        private void OnGameFinished() => enabled = false;

        private void OnCollisionGround(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Ground _))
                CollidedWithGround?.Invoke();
        }

        private void OnCollisionObstacle(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Obstacle obstacle))
                CollidedWithObstacle?.Invoke(obstacle);
        }

        private void OnTriggerEnterCar(Collider other)
        {
            if (other.TryGetComponent(out Car.Car car))
                TriggeredEnterWithCar?.Invoke(car);
        }

        private void OnTriggerEnterBirck(Collider other)
        {
            if (other.TryGetComponent(out Brick brik))
                TriggeredWithBrick?.Invoke(brik);
        }

        private void OnTriggerEnterWall(Collider other)
        {
            if (other.TryGetComponent(out Wall.Wall wall))
                TriggeredWithWall?.Invoke(wall);
        }

        private void OnTriggerEnterCameraTrigger(Collider other)
        {
            if (other.TryGetComponent(out CameraTrigger cameraTrigger))
                TriggeredWithCameraTrigger?.Invoke(cameraTrigger);
        }

        private void OnTriggerEnterNextTile(Collider other)
        {
            if (other.TryGetComponent(out TriggerNextTile _))
                TriggeredNextTile?.Invoke();
        }
    }
}