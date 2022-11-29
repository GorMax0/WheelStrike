using System;
using Services.GameStates;
using UI;
using UI.Views;

namespace Core
{
    public class Finish : IDisposable
    {
        private GameStateService _gameStateService;
        private ViewValidator _validator;
        private FinishView _finishView;
        private bool _isFinished;

        public Finish(GameStateService gameStateService, ViewValidator validator)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _validator = validator;
            _validator.ViewValidated += OnViewValidated;
        }

        public void Dispose()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _validator.ViewValidated -= OnViewValidated;
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
            switch(state)
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
            _isFinished = true;
        }
    }
}