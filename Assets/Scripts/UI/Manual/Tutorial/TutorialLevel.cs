using System;
using Achievements;
using Core;
using GameAnalyticsSDK;
using Services;
using Services.GameStates;
using UI.Views;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Manual.Tutorial
{
    public class TutorialLevel : MonoBehaviour, IPointerDownHandler
    {
        private const string TutorialData = "Tutorial";
        private const int Step0 = 0;
        private const int Step2 = 2;
        private const int Step3 = 3;
        private const int Step4 = 4;
        private const int Step5 = 5;
        private const int Step6 = 6;
        private const int Step10 = 10;
        private const int Step11 = 11;
        [SerializeField] private StepTutorial[] _portraitSteps;
        [SerializeField] private StepTutorial[] _landscapeSteps;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private Wrap _parametersShop;
        [SerializeField] private Image _overlayingSubstrate;

        private GameStateService _gameStateService;
        private TutorialState _currentState;
        private StepTutorial _currentStep;
        private AchievementSystem _achievementSystem;
        private Image _image;
        private int _indexStep;
        private bool _hasPortraitOrientation;
        private bool _isInitialize;

        public int TutorialComplete { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_indexStep == Step0)
            {
                _gameStateService.ChangeState(GameState.TutorialStepZero);
                _indexStep++;
                SwitchCurrentStep();
                _image.raycastTarget = false;
            }
        }

        public void Initialize(
            GameStateService gameStateService,
            TutorialState state,
            AchievementSystem achievementSystem)
        {
            if (_isInitialize)
            {
                throw new InvalidOperationException(
                    $"{GetType()}: Initialize(GameStateService gameStateService, TutorialState state): Already initialized.");
            }

            Localization.SetLanguage();
            _image = GetComponent<Image>();
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _currentState = state;
            _achievementSystem = achievementSystem;

            _inputHandler.gameObject.SetActive(false);
            _topPanel.gameObject.SetActive(false);
            _menuView.gameObject.SetActive(false);
            ScreenOrientationValidator.Instance.OrientationValidated += OnOrientationValidated;
            _isInitialize = true;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, TutorialData);

            StartTutorial(_currentState);
        }

        private void SaveTutorialProgress()
        {
            PlayerPrefs.SetInt(TutorialData, (int)_currentState);
            PlayerPrefs.Save();
        }

        private void StartTutorial(TutorialState _currentState)
        {
            switch (_currentState)
            {
                case TutorialState.HalfCompleted:
                    _indexStep = Step5;

                    break;
                case TutorialState.FullCompleted:
                    return;
                case TutorialState.Start:
                default:
                    _indexStep = Step0;

                    break;
            }

            SetCurrentStep();
            _currentStep.Enable();
        }

        private void SetCurrentStep()
        {
            if (_indexStep >= _portraitSteps.Length || _portraitSteps[_indexStep] == null)
                return;

            if (_currentStep != null)
                _currentStep.StepCompleted -= OnStepCompleted;

            _currentStep = _hasPortraitOrientation ? _portraitSteps[_indexStep] : _landscapeSteps[_indexStep];
            _currentStep.StepCompleted += OnStepCompleted;
        }

        private void SwitchCurrentStep()
        {
            _currentStep.Disable();
            SetCurrentStep();

            if (_indexStep != Step4)
                _currentStep.Enable();

            if (_indexStep > Step11)
                FinishTutorial();
        }

        private void FinishTutorial()
        {
            TutorialComplete = 1;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, TutorialData);
            _gameStateService.ChangeState(GameState.Save);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void OnStepCompleted()
        {
            _indexStep++;

            switch (_indexStep)
            {
                case Step2:
                    _gameStateService.ChangeState(GameState.TutorialStepTwo);

                    break;
                case Step3:
                    _gameStateService.ChangeState(GameState.TutorialStepThree);

                    break;
                case Step4:
                    _gameStateService.ChangeState(GameState.TutorialStepFour);
                    _topPanel.gameObject.SetActive(true);
                    _inputHandler.gameObject.SetActive(true);

                    break;
                case Step6:
                    _overlayingSubstrate.gameObject.SetActive(true);
                    _parametersShop.ApplyOffsetTransform();

                    break;
                case Step10:
                    _parametersShop.CancelOffsetTransform();

                    break;
            }

            SwitchCurrentStep();
        }

        private void OnOrientationValidated(bool isPortrait)
        {
            if (_hasPortraitOrientation == isPortrait || _indexStep == Step4)
                return;

            if (_currentStep != null && _currentStep.IsAction)
                _currentStep.Disable();

            _hasPortraitOrientation = isPortrait;
            SetCurrentStep();

            _currentStep.Enable();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Restart:
                    OnGameRestart();

                    break;
            }
        }

        private void OnGameRestart()
        {
            _indexStep++;
            _currentState = TutorialState.HalfCompleted;
            SaveTutorialProgress();
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }
    }
}