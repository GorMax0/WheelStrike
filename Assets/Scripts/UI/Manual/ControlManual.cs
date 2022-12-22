using TMPro;
using UnityEngine;
using Services.GameStates;

namespace UI.Manual
{
    public class ControlManual : MonoBehaviour
    {
        [SerializeField] private AimManual _aimManual;
        [SerializeField] private TMP_Text _holdToPlay;

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
                case GameState.Waiting:
                    OnGameWaiting();
                    break;
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGameWaiting()
        {
            _holdToPlay.gameObject.SetActive(false);
            _aimManual.gameObject.SetActive(true);
            _aimManual.StartTween();
        }

        private void OnGameRunning()
        {
            _aimManual.Fade();
        }
    }
}