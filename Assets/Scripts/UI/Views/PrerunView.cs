using Services.GameStates;
using UnityEngine;

namespace UI.Views
{
    public class PrerunView : MonoBehaviour
    {
        [SerializeField] private ForceScaleView _forceScaleView;
        [SerializeField] private AimDirectionView _aimDirectionLine;

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
                case GameState.Initializing:
                    OnGameInitializing();

                    break;
                case GameState.Waiting:
                    OnGameWaiting();

                    break;
                case GameState.Running:
                    OnGameRunning();

                    break;
                case GameState.TutorialStepZero:
                    OnGameTutorialStepZero();

                    break;
                case GameState.TutorialStepTwo:
                    OnGameTutorialStepTwo();

                    break;
                case GameState.TutorialStepThree:
                    OnGameTutorialStepThree();

                    break;
            }
        }

        private void OnGameInitializing()
        {
            _forceScaleView.gameObject.SetActive(false);
            _aimDirectionLine.gameObject.SetActive(false);
        }

        private void OnGameWaiting()
        {
            _forceScaleView.gameObject.SetActive(true);
            _aimDirectionLine.gameObject.SetActive(true);
        }

        private void OnGameRunning()
        {
            _forceScaleView.Fade();
            _aimDirectionLine.gameObject.SetActive(false);
        }

        private void OnGameTutorialStepZero()
        {
            _forceScaleView.gameObject.SetActive(true);
        }

        private void OnGameTutorialStepTwo()
        {
            _aimDirectionLine.gameObject.SetActive(true);
        }

        private void OnGameTutorialStepThree()
        {
            _forceScaleView.gameObject.SetActive(false);
            _aimDirectionLine.gameObject.SetActive(false);
        }
    }
}