using System.Collections.Generic;
using System;
using System.Linq;
using Services.GameStates;
using UnityEngine;

namespace Trail
{
    public class TrailManager : MonoBehaviour
    {
        [SerializeField] private List<TrailFX> _trails;

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

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameStateRunning();
                    break;
                case GameState.Finished:
                    OnGameStateFinished();
                    break;
            }
        }

        private void OnGameStateRunning()
        {
            _currentTrail = FindSelectedTrail();
            _currentTrail.gameObject.SetActive(true);
        }

        private void OnGameStateFinished()
        {
            _currentTrail.gameObject.SetActive(false);
        }
    }
}