using UnityEngine;
using Services.GameStates;

namespace UI.Views
{
    public class PrerunView : MonoBehaviour
    {
        [SerializeField] private ForceScaleView _forceScaleView;

        private GameStateService _gameStateService;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            OnEnable();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Pause:
                    OnGamePause();
                    break;
                case GameState.Waiting:
                    OnGameWaiting();
                    break;
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGamePause()
        {
            _forceScaleView.gameObject.SetActive(false);
        }

        private void OnGameWaiting()
        {
            _forceScaleView.gameObject.SetActive(true);
        }

        private void OnGameRunning()
        {
            _forceScaleView.Fade();
        }
    }
}