using Services.GameStates;
using UnityEngine;

namespace UI.Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Wrap _levelLable;
        [SerializeField] private Wrap _achievementButton;
        [SerializeField] private Wrap _boostButton;
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
        }

        private void OnGameWaiting()
        {
            _achievementButton.ApplyOffsetTransform();
            _boostButton.ApplyOffsetTransform();
            _parametersShop.ApplyOffsetTransform();
            _levelLable.ApplyOffsetTransform();
        }
    }
}