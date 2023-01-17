using UnityEngine;
using Services.GameStates;

namespace UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Wrap _levelLable;
        [SerializeField] private Wrap _worldButton;
        [SerializeField] private Wrap _shopButton;
        [SerializeField] private Wrap _leaderboardButton;
        [SerializeField] private Wrap _parametersShop;

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
            }

            Debug.Log(state);
        }

        private void OnGameWaiting()
        {
            _worldButton.ApplyOffsetTransform();
            _shopButton.ApplyOffsetTransform();
            _leaderboardButton.ApplyOffsetTransform();
            _parametersShop.ApplyOffsetTransform();
            _levelLable.ApplyOffsetTransform();
        }
    }
}