using System;
using System.Collections;
using UnityEngine;
using Services.GameStates;
using Services.Coroutines;
using UI.Views;

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
        private bool _isFinished;

        public FinishViewHandler(GameStateService gameStateService, CoroutineService coroutineService, ViewValidator validator, ITravelable travelable)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _coroutineService = coroutineService;
            _validator = validator;
            _validator.ViewValidated += OnViewValidated;
            _travelable = travelable;

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
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.07f);
            float distanceTreveled = _travelable.TraveledDistance;
            float displayedDistance = 0f;
            float timeOfMaximum = 14f;
            float step = distanceTreveled / timeOfMaximum;

            while (displayedDistance < distanceTreveled)
            {
                yield return waitForSeconds;
                displayedDistance += Mathf.MoveTowards(displayedDistance, distanceTreveled, step);
                DisplayedDistanceChanged?.Invoke(displayedDistance);
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