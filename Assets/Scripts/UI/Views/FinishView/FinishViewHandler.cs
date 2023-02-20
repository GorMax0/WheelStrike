using System;
using System.Collections;
using UnityEngine;
using Core.Wheel;
using Services;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;
using AdsReward;
using Leaderboards;
using GameAnalyticsSDK;

namespace UI.Views.Finish
{
    [RequireComponent(typeof(FinishViewTopLabelSetter))]
    public class FinishViewHandler : MonoBehaviour
    {
        [SerializeField] private FinishView _viewPortrait;
        [SerializeField] private FinishView _viewLandscape;
        [Range(0.001f, 0.05f)]
        [SerializeField] private float _waitingDelayDisplayValue;
        [SerializeField] private ParticleSystem _finishEffect;
        [SerializeField] private AdsRewards _adsRewards;
        [SerializeField] private LeaderboardsHandler _leaderboardsHandler;

        private const int ValueIfInfinity = 5000;

        private GameStateService _gameStateService;
        private CoroutineService _coroutineService;
        private RewardScaler _rewardScaler;
        private CoroutineRunning _distanceDisplay;
        private CoroutineRunning _scoreDisplay;
        private CoroutineRunning _bonusScoreDisplay;
        private FinishViewTopLabelSetter _topLabelSetter;
        private FinishView _currentFinishView;
        private ITravelable _travelable;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private bool _isInitialized;
        private bool _hasPortraitOrientation = true;
        private bool _isLevelInfinity;
        private bool _isFinished;
        private bool _hasOpenVideoAd;

        public event Action<int> DisplayedDistanceChanged;
        public event Action<int> DisplayedRewardChanged;
        public event Action<int> DisplayedBonusRewardChanged;
        public event Action<int> DisplayedHighscoreChanged;
        public event Action<int> DisplayedHighscoreLoaded;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            ScreenOrientationValidator.Instance.OrientationValidated += OnOrientationValidated;
            _levelScore.HighscoreChanged += OnHighscoreChanged;
            _levelScore.HighscoreLoaded += OnHighscoreLoaded;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            ScreenOrientationValidator.Instance.OrientationValidated -= OnOrientationValidated;
            _levelScore.HighscoreChanged -= OnHighscoreChanged;
            _levelScore.HighscoreLoaded -= OnHighscoreLoaded;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, ITravelable travelable, LevelService levelService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, CoroutineService coroutineService, ITravelable travelable, LevelService levelService): Already initialized.");

            _topLabelSetter = GetComponent<FinishViewTopLabelSetter>();
            _gameStateService = gameStateService;
            _coroutineService = coroutineService;
            _travelable = travelable;
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _isLevelInfinity = _levelService.IsInfinity;
            _rewardScaler = new RewardScaler(gameStateService, coroutineService);

            InitializeViews();

            _distanceDisplay = new CoroutineRunning(_coroutineService);
            _scoreDisplay = new CoroutineRunning(_coroutineService);
            _bonusScoreDisplay = new CoroutineRunning(_coroutineService);
            _isInitialized = true;
            OnEnable();
        }

        public void DisplayDistance() => _distanceDisplay.Run(DisplayValue(_travelable.DistanceTraveled, DisplayedDistanceChanged));

        public void DisplayReward() => _scoreDisplay.Run(DisplayValue(_levelScore.Reward, DisplayedRewardChanged));

        public void DisplayBonusReward() => _bonusScoreDisplay.Run(DisplayValue(_levelScore.BonusReward, DisplayedBonusRewardChanged));

        public void OnAdsButtonClick()
        {
            _rewardScaler.StopTween();
#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("OnAdsButtonClicked");
#elif YANDEX_GAMES
            Agava.YandexGames.VideoAd.Show(OnOpenCallback, OnRewardedCallback, OnCloseCallback, OnErrorCallback);
#endif
        }

        private void InitializeViews()
        {
            int sliderLength = _isLevelInfinity == false ? _levelService.LengthRoad : Mathf.RoundToInt(_travelable.DistanceTraveled / 100) * 100 + ValueIfInfinity;
            _viewPortrait.Initialize(this, _rewardScaler, sliderLength);
            _viewLandscape.Initialize(this, _rewardScaler, sliderLength);

            _currentFinishView = _hasPortraitOrientation == true ? _viewPortrait : _viewLandscape;
        }

        private IEnumerator DisplayValue(float endValue, Action<int> displayedValueChanged)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_waitingDelayDisplayValue);
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

        private void PauseOn()
        {
            SoundController.ChangeWhenAd(true);
            Time.timeScale = 0f;
        }

        private static void PauseOff()
        {
            SoundController.ChangeWhenAd(false);
            Time.timeScale = 1f;
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

        private void OnHighscoreChanged(int newHighscore) => DisplayedHighscoreChanged?.Invoke(newHighscore);

        private void OnHighscoreLoaded(int highscore) => DisplayedHighscoreLoaded?.Invoke(highscore);

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Finished:
                    Invoke(nameof(OnGameFinished), 0.3f);
                    break;
                case GameState.Restart:
                    OnGameRestart();
                    break;
            }
        }

        private void OnGameFinished()
        {
            InitializeViews();
            _finishEffect.Play();
            _currentFinishView.Enable();
            _topLabelSetter.SelectLabel(_travelable.DistanceTraveled, _levelService.LengthRoad);
            _currentFinishView.StartAnimation();
            _leaderboardsHandler?.SaveScore();

            _isFinished = true;
        }

        private void OnGameRestart() => _rewardScaler.StopTween();

        private void OnOpenCallback()
        {
            PauseOn();
            _hasOpenVideoAd = true;
            GameAnalytics.NewDesignEvent("AdClick:RewardMultiplier");
        }

        private void OnRewardedCallback()
        {
            if (_hasOpenVideoAd == false)
                return;

            _levelScore.SetAdsRewardRate(_rewardScaler.CurrentRate);
            _adsRewards.EnrollReward(RewardType.Money, _levelScore.ResultReward);
            _hasOpenVideoAd = false;
        }

        private void OnCloseCallback() => PauseOff();

        private void OnErrorCallback(string message)
        {
            if (_hasOpenVideoAd == true)
                return;

            _adsRewards.ShowErrorAds();
            Debug.LogWarning(message);
            PauseOff();
        }
    }
}