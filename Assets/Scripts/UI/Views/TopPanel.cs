using UnityEngine;
using UnityEngine.UI;
using Services.GameStates;

namespace UI.Views
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private Button _settingButton;
        [SerializeField] private Wrap _restartButton;
        [SerializeField] private Wrap _moneyPanel;
        [SerializeField] private Wrap _distancePanel;

        private GameStateService _gameStateService;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.RemoveListener(Restart);
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;          

            OnEnable();
        }

        public void Restart()
        {
            _gameStateService.ChangeState(GameState.Restart);
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
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameWaiting()
        {            
            _moneyPanel.ApplyOffsetTransform();
        }
        private void OnGameRunning()
        {
            _restartButton.ApplyOffsetTransform();
            _distancePanel.ApplyOffsetTransform();
        }

        private void OnGameFinished()
        {
            _distancePanel.CancelOffsetTransform();
            _restartButton.CancelOffsetTransform();
        }
    }
}