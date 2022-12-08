using System;
using System.Collections;
using UnityEngine;
using Core.Wheel;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;

namespace UI.Views.Finish
{
    [RequireComponent(typeof(ViewValidator))]
    public class FinishViewHandler : MonoBehaviour
    {
        [SerializeField] private FinishView[] _finishViews;
        [Range(0.001f,0.05f)]
        [SerializeField] private float _waitingDelay;

        private GameStateService _gameStateService;
        private CoroutineService _coroutineService;
        private CoroutineRunning _distanceDisplay;
        private CoroutineRunning _scoreDisplay;
        private CoroutineRunning _bonusScoreDisplay;
        private ViewValidator _validator;
        private FinishView _currentFinishView;
        private ITravelable _travelable;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private bool _isInitialized;
        private bool _isFinished;

        public event Action<int> DisplayedDistanceChanged;
        public event Action<int> DisplayedScoreChanged;
        public event Action<int> DisplayedBonusScoreChanged;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _validator.ViewValidated += OnViewValidated;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _validator.ViewValidated -= OnViewValidated;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, ITravelable travelable, LevelService levelService)
        {
            _validator = GetComponent<ViewValidator>();
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
            foreach (FinishView view in _finishViews)
            {
                view.Initialize(this, _levelService.LengthRoad);
            }
        }

        private IEnumerator DisplayValue(float endValue, Action<int> displayedValueChanged)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_waitingDelay);
            float displayedValue = 0f;
            float dividerForStep = 14f;
            float step = endValue / dividerForStep;

            while (displayedValue < endValue)
            {
                displayedValue = Mathf.MoveTowards(displayedValue,endValue,step);
                displayedValueChanged?.Invoke((int)displayedValue);
                yield return waitForSeconds;
            }
        }

        private void OnViewValidated(FinishView finishView)
        {
            if (_currentFinishView == finishView)
                return;

            if (_currentFinishView != null && _currentFinishView.IsAction == true)
                _currentFinishView.Disable();

            _currentFinishView = finishView;

            if (_isFinished == true)
                _currentFinishView.Enable();
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameFinished()
        {
            _currentFinishView.gameObject.SetActive(true);
            _currentFinishView.StartAnimation();
            
            _isFinished = true;
        }
    }
}