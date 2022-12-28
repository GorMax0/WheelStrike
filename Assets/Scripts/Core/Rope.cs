using UnityEngine;
using Obi;
using Core.Wheel;
using Services.GameStates;
using System;

namespace Core
{
    [RequireComponent(typeof(ObiParticleAttachment))]
    public class Rope : MonoBehaviour
    {
        [SerializeField] private Movement _jointObject;
        [SerializeField] private ObiLateUpdater _lateUpdater;

        private ObiParticleAttachment[] _joints;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        private void Awake()
        {
            _joints = GetComponents<ObiParticleAttachment>();
        }

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
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService): Already initialized.");

            _gameStateService = gameStateService;
            _isInitialized = true;
            OnEnable();
        }

        private void Destroy()
        {
            _lateUpdater.enabled = false;
            Destroy(gameObject);
        }

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

            foreach (ObiParticleAttachment joint in _joints)
            {
                if (joint.target == _jointObject.transform)
                    joint.enabled = false;
            }

            Invoke(nameof(Destroy), deleyDestroy);
        }
    }
}