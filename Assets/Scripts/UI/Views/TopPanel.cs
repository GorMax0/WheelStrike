using UnityEngine;
using UnityEngine.UI;
using Services;
using Services.GameStates;

namespace UI.Views
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private Button _settingButton;
        [SerializeField] private Wrap _restartButton;
        [SerializeField] private Wrap _moneyPanel;

        private GameStateService _gameStateService;
        private LevelService _levelService;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.AddListener(_levelService.RestartLevel);
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.RemoveListener(_levelService.RestartLevel);
        }

        public void Initialize(GameStateService gameStateService, LevelService levelService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            _levelService = levelService;

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
            _restartButton.Unroll();
            _moneyPanel.Unroll();
        }

        private void OnGameWaiting()
        {
            _restartButton.Roll();
            _moneyPanel.Roll();
        }
        private void OnGameRunning()
        {
            _restartButton.Unroll();
        }

    }
}