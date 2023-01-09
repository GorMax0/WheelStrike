using System;
using System.Collections;
using UnityEngine;
using Core.Wheel;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;

namespace UI.Views.Finish
{
    [RequireComponent(typeof(ScreenOrientationValidator))]
    public class FinishViewHandler : MonoBehaviour
    {
        [SerializeField] private FinishView _viewPortrait;
        [SerializeField] private FinishView _viewLandscape;
        [Range(0.001f, 0.05f)]
        [SerializeField] private float _waitingDelay;
        [SerializeField] private ParticleSystem _finishEffect;

        private GameStateService _gameStateService;
        private CoroutineService _coroutineService;
        private CoroutineRunning _distanceDisplay;
        private CoroutineRunning _scoreDisplay;
        private CoroutineRunning _bonusScoreDisplay;
        private ScreenOrientationValidator _validator;
        private FinishView _currentFinishView;
        private ITravelable _travelable;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private bool _isInitialized;
        private bool _hasPortraitOrientation;
        private bool _isFinished;

        public event Action<int> DisplayedDistanceChanged;
        public event Action<int> DisplayedScoreChanged;
        public event Action<int> DisplayedBonusScoreChanged;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _validator.OrientationValidated += OnOrientationValidated;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _validator.OrientationValidated -= OnOrientationValidated;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, ITravelable travelable, LevelService levelService)
        {
            _validator = GetComponent<ScreenOrientationValidator>();
            _gameStateService = gameStateService;
            _coroutineService = coroutineService;
            _travelable = travelable;
            _levelService = levelService;
            _levelScore = _levelService.Score;

            InitializeViews();

            _distanceDisplay = new CoroutineRunning(_coroutineService);
            _scoreDisplay = new CoroutineRunning(_coroutineService);
            _bonusScoreDisplay = new CoroutineRunning(_coroutineService);
            _isInitialized = true;
            OnEnable();
        }

        public void DisplayDistance()
        {
            float distanceTraveled = _travelable.DistanceTraveled;
            _distanceDisplay.Run(DisplayValue(distanceTraveled, DisplayedDistanceChanged));
        }

        public void DisplayScore()
        {
            float score = _levelScore.Score;
            _scoreDisplay.Run(DisplayValue(score, DisplayedScoreChanged));
        }

        public void DisplayBonusScore()
        {
            float bonusScore = _levelScore.BonusScore;
            _bonusScoreDisplay.Run(DisplayValue(bonusScore, DisplayedBonusScoreChanged));
        }

        private void InitializeViews()
        {
            _viewPortrait.Initialize(this, _levelService.LengthRoad);
            _viewLandscape.Initialize(this, _levelService.LengthRoad);

            _currentFinishView = _viewPortrait;
            _hasPortraitOrientation = true;
        }

        private IEnumerator DisplayValue(float endValue, Action<int> displayedValueChanged)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_waitingDelay);
            float displayedValue = 0f;
            float dividerForStep = 14f;
            float step = endValue / dividerForStep;

            while (displayedValue < endValue)
            {
                displayedValue = Mathf.MoveTowards(displayedValue, endValue, step);
                displayedValueChanged?.Invoke((int)displayedValue);
                yield return waitForSeconds;
            }
        }

        private void OnOrientationValidated(bool isPortrait)
        {
            if (_hasPortraitOrientation == isPortrait)
                return;

            if (_currentFinishView != null && _currentFinishView.IsAction == true)
                _currentFinishView.Disable();

            _hasPortraitOrientation = isPortrait;
            _currentFinishView = _hasPortraitOrientation == true ? _viewPortrait : _viewLandscape;

            if (_isFinished == true)
                _currentFinishView.Enable();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Finished:
                    Invoke(nameof(OnGameFinished), 0.5f); //Создать единую константу для 0.5f;
                    break;
            }
        }

        private void OnGameFinished()
        {
            _finishEffect.Play();
            _currentFinishView.gameObject.SetActive(true);
            _currentFinishView.StartAnimation();

            _isFinished = true;
        }
    }
}