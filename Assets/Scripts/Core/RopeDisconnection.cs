using System;
using RopeMinikit;
using Services.GameStates;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(RopeConnection))]
    public class RopeDisconnection : MonoBehaviour
    {
        [SerializeField] private Transform _connectionPointLeft;
        [SerializeField] private Transform _connectionPointRight;

        private RopeConnection[] _joints;
        private GameStateService _gameStateService;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized)
                throw new InvalidOperationException(
                    $"{GetType()}: Initialize(GameStateService gameStateService): Already initialized.");

            _joints = GetComponents<RopeConnection>();
            _gameStateService = gameStateService;
            _isInitialized = true;
            OnEnable();
        }

        private void Destroy() => Destroy(gameObject);

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Running:
                    OnGameRunning();

                    break;
            }
        }

        private void OnGameRunning()
        {
            float deleyDestroy = 0.8f;

            foreach (RopeConnection joint in _joints)
            {
                if (joint.transformSettings.transform == _connectionPointLeft
                    || joint.transformSettings.transform == _connectionPointRight)
                {
                    joint.enabled = false;
                }
            }

            Invoke(nameof(Destroy), deleyDestroy);
        }
    }
}