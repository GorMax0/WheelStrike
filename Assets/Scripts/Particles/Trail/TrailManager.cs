using System;
using System.Collections.Generic;
using System.Linq;
using Services.GameStates;
using UnityEngine;
using Core.Wheel;

namespace Trail
{
    public class TrailManager : MonoBehaviour
    {
        [SerializeField] private List<TrailFX> _trails;
        [SerializeField] private Movement _wheel;

        private const float DelayInvoke = 0.5f;

        private TrailFX _currentTrail;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService): Already initialized.");

            _gameStateService = gameStateService;
            _isInitialized = true;
            OnEnable();
        }

        private TrailFX FindSelectedTrail() => _trails.Where(trail => trail.IsSelected).First();

        private void PassWheel() => _currentTrail.SetWheel(_wheel);

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Finished:
                    Invoke(nameof(OnGameFinished), DelayInvoke);
                    break;
            }
        }

        private void OnGameRunning()
        {
            _currentTrail = FindSelectedTrail();
            PassWheel();
            _currentTrail.gameObject.SetActive(true);
        }

        private void OnGameFinished() => _currentTrail.gameObject.SetActive(false);
    }
}