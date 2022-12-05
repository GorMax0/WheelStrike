using System;
using System.Collections;
using UnityEngine;
using Core.Wheel;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;

namespace UI.Views.Finish
{
    public class FinishViewHandler : IDisposable
    {
        private GameStateService _gameStateService;
        private CoroutineService _coroutineService;
        private CoroutineRunning _distanceDisplay;
        private ViewValidator _validator;
        private FinishView _finishView;
        private ITravelable _travelable;
        private LevelScore _levelScore;
        private bool _isFinished;

        public FinishViewHandler(GameStateService gameStateService, CoroutineService coroutineService, ViewValidator validator, ITravelable travelable, LevelScore levelScore)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _coroutineService = coroutineService;
            _validator = validator;
            _validator.ViewValidated += OnViewValidated;
            _travelable = travelable;
            _levelScore = levelScore;

            _distanceDisplay = new CoroutineRunning(_coroutineService);
        }

        public event Action<float> DisplayedDistanceChanged;

        public void Dispose()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _validator.ViewValidated -= OnViewValidated;
        }

        private IEnumerator DisplayDistance()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
            float distanceTreveled = _travelable.TraveledDistance;
            float displayedValue = 0f;
            float timeOfMaximum = 14f;
            float step = distanceTreveled / timeOfMaximum;

            while (displayedValue < distanceTreveled)
            {
                yield return waitForSeconds;
                displayedValue += Mathf.MoveTowards(displayedValue, distanceTreveled, step);
                DisplayedDistanceChanged?.Invoke(displayedValue);
            }
        }

        private void OnViewValidated(FinishView finishView)
        {
            if (_finishView == finishView)
                return;

            if (_finishView != null && _finishView.gameObject.activeInHierarchy == true)
                _finishView.gameObject.SetActive(false);

            _finishView = finishView;

            if (_isFinished == true)
                _finishView.gameObject.SetActive(true);
        }

        private void OnGameStateChanged(GameState state)
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
            _finishView.gameObject.SetActive(true);
            _finishView.StartAnimation();
            _distanceDisplay.Run(DisplayDistance());
            _isFinished = true;
        }
    }
}