using TMPro;
using UnityEngine;
using Zenject;
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
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGamePause()
        {
            _aimManual.gameObject.SetActive(false);
            _holdToPlay.gameObject.SetActive(true);
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