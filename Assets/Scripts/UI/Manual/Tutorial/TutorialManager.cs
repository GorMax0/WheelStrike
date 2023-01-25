using UnityEngine;
using Parameters;
using Services;
using Services.GameStates;
using UI.Views;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Core;
using UnityEngine.SceneManagement;

namespace UI.Manual.Tutorial
{
    public class TutorialManager : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private StepTutorial[] _portraitSteps;
        [SerializeField] private StepTutorial[] _landscapeSteps;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private Wrap _parametersShop;
        [SerializeField] private Image _overlayingSubstrate;

        private const string TutorialData = "Tutorial";
        private const string NextSceneName = "Level1";
        private const int Step0 = 0;
        private const int Step2 = 2;
        private const int Step3 = 3;
        private const int Step4 = 4;
        private const int Step5 = 5;    
        private const int Step9 = 9;

        private GameStateService _gameStateService;
        private TutorialState _currentState;
        private StepTutorial _currentStep;
        private Image _image;
        private int _indexStep;
        private bool _hasPortraitOrientation;
        private bool _isCompletedTutorial;
        private bool _isInitialize;

        public void Initialize(GameStateService gameStateService, TutorialState state)
        {
            if (_isInitialize == true)
                throw new System.InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, TutorialState state): Already initialized.");

            _image = GetComponent<Image>();
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _currentState = state;

            _inputHandler.gameObject.SetActive(false);
            _topPanel.gameObject.SetActive(false);
            _menuView.gameObject.SetActive(false);
            ScreenOrientationValidator.Instance.OrientationValidated += OnOrientationValidated;
            _isInitialize = true;
            Debug.Log($"Initialize {_currentState}");
            StartTutorial(_currentState);
        }

        private void SaveTutorialProgress()
        {
            Debug.Log($"Save {_currentState}");
            PlayerPrefs.SetInt(TutorialData, (int)_currentState);
            PlayerPrefs.Save();
        }

        private void StartTutorial(TutorialState _currentState)
        {
            switch (_currentState)
            {
                case TutorialState.HalfCompleted:
                    _indexStep = Step4;
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
            if (_indexStep >= _portraitSteps.Length)
                return;

            if (_currentStep != null)
                _currentStep.StepCompleted -= OnStepCompleted;

            _currentStep = _hasPortraitOrientation == true ? _portraitSteps[_indexStep] : _landscapeSteps[_indexStep];
            _currentStep.StepCompleted += OnStepCompleted;
        }

        private void SwitchCurrentStep()
        {
            _currentStep.Disable();
            SetCurrentStep();

            if (_indexStep != Step4)
                _currentStep.Enable();

            if (_indexStep > Step9)
                FinishTutorial();
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
                case Step5:
                    _overlayingSubstrate.gameObject.SetActive(true);
                    _parametersShop.ApplyOffsetTransform();
                    break;
                case Step9:
                    _parametersShop.CancelOffsetTransform();
                    break;
            }

            SwitchCurrentStep();
        }

        private void FinishTutorial()
        {
            _currentState = TutorialState.FullCompleted;
            SaveTutorialProgress();
            SceneManager.LoadScene(NextSceneName);
        }

        private void OnOrientationValidated(bool isPortrait)
        {
            if (_hasPortraitOrientation == isPortrait)
                return;

            if (_currentStep != null && _currentStep.IsAction == true)
                _currentStep.Disable();

            _hasPortraitOrientation = isPortrait;
            SetCurrentStep();

            if (_isCompletedTutorial == false)
                _currentStep.Enable();
        }

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
            _currentState = TutorialState.HalfCompleted;
            SaveTutorialProgress();
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }
    }
}