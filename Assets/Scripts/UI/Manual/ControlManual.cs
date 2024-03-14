using Services;
using Services.Coroutines;
using Services.GameStates;
using UnityEngine;

namespace UI.Manual
{
    public class ControlManual : MonoBehaviour
    {
        [SerializeField] private AimManual _aimManual;
        [SerializeField] private GameObject _holdToPlayPortrait;
        [SerializeField] private GameObject _holdToPlayLandscape;
        [SerializeField] private bool _isTutorial;

        private GameStateService _gameStateService;
        private GameObject _holdToPlay;
        private bool _hasPortraitOrientation;

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

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            _aimManual.Initialize(coroutineService);
            _holdToPlay = Screen.width < Screen.height ? _holdToPlayPortrait : _holdToPlayLandscape;
            ScreenOrientationValidator.Instance.OrientationValidated += OnOrientationValidated;
            _holdToPlay.SetActive(!_isTutorial);
            OnEnable();
        }

        private void ActivateAimManual()
        {
            _aimManual.gameObject.SetActive(true);
            _aimManual.Display();
            _aimManual.StartTween();
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
                case GameState.TutorialStepTwo:
                    OnGameTutorialStepTwo();

                    break;
                case GameState.TutorialStepThree:
                    OnGameTutorialStepThree();

                    break;
                case GameState.TutorialStepFour:
                    OnGameTutorialStepFour();

                    break;
            }
        }

        private void OnGameWaiting()
        {
            _holdToPlay.SetActive(false);
            ActivateAimManual();
            ScreenOrientationValidator.Instance.OrientationValidated -= OnOrientationValidated;
        }

        private void OnGameRunning() => _aimManual.Fade();

        private void OnGameTutorialStepTwo() => ActivateAimManual();

        private void OnGameTutorialStepThree() => _aimManual.Fade();

        private void OnGameTutorialStepFour() => _holdToPlay.SetActive(true);

        private void OnOrientationValidated(bool isPortraitOrientation)
        {
            if (_hasPortraitOrientation == isPortraitOrientation || _holdToPlay.activeInHierarchy == false)
                return;

            _holdToPlay.SetActive(false);
            _holdToPlay = isPortraitOrientation ? _holdToPlayPortrait : _holdToPlayLandscape;
            _holdToPlay.SetActive(true);
            _hasPortraitOrientation = isPortraitOrientation;
        }
    }
}