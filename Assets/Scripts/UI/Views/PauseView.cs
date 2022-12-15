using TMPro;
using UnityEngine;
using Services.GameStates;

namespace UI.Views
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private Wrap _worldButton;
        [SerializeField] private Wrap _shopButton;
        [SerializeField] private Wrap _parametersShop;
        [SerializeField] private TMP_Text _levelLable;

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
            }

            Debug.Log(state);
        }

        private void OnGamePause()
        {
            _worldButton.Unroll();
            _shopButton.Unroll();
            _parametersShop.Unroll();
            _levelLable.gameObject.SetActive(true);
        }

        private void OnGameWaiting()
        {
            _worldButton.Roll();
            _shopButton.Roll();
            _parametersShop.Roll();
            _levelLable.gameObject.SetActive(false);
        }
    }
}