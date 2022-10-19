using TMPro;
using UnityEngine;
using Zenject;
using Services.GameStates;

namespace UI.Views
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField] private Wrap _worldButton;
        [SerializeField] private Wrap _shopButton;
        [SerializeField] private Wrap _upgradePanel;
        [SerializeField] private TMP_Text _levelLable;

        private GameStateService _gameStateService;

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        [Inject]
        private void Construct(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
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
            _upgradePanel.Unroll();
            _levelLable.gameObject.SetActive(true);
        }

        private void OnGameWaiting()
        {
            _worldButton.Roll();
            _shopButton.Roll();
            _upgradePanel.Roll();
            _levelLable.gameObject.SetActive(false);
        }
    }
}