using System;
using System.Collections;
using UnityEngine;
using Core.Wheel;
using Services;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;
using AdsReward;
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
        private bool _hasPortraitOrientation;
        private bool _isFinished;

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
            Agava.YandexGames.VideoAd.Show(onOpenCallback: OnOpenCallback, onRewardedCallback: OnRewardedCallback, onCloseCallback: OnCloseCallback);
#endif
            // -------------> if(Agava.WebUtility.AdBlock.Enabled)
        }

        private void InitializeViews()
        {
            _viewPortrait.Initialize(this, _rewardScaler, _levelService.LengthRoad);
            _viewLandscape.Initialize(this, _rewardScaler, _levelService.LengthRoad);

            _currentFinishView = _viewPortrait;
            _hasPortraitOrientation = true;
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
            }
        }

        private void OnGameFinished()
        {
            _finishEffect.Play();
            _currentFinishView.Enable();
            _topLabelSetter.SelectLabel(_travelable.DistanceTraveled, _levelService.LengthRoad);
            _currentFinishView.StartAnimation();

            _isFinished = true;
        }

        private void OnOpenCallback()
        {
            SoundController.ChangeWhenAd(true);
            Time.timeScale = 0f;
            GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "YandexSDK", $"Yandex");
            GameAnalytics.NewDesignEvent("AdClick:RewardMultiplier");
        }

        private void OnCloseCallback()
        {
            SoundController.ChangeWhenAd(false);
            Time.timeScale = 1f;
        }

        private void OnRewardedCallback()
        {
            _levelScore.SetAdsRewardRate(_rewardScaler.CurrentRate);
            _adsRewards.EnrollReward(RewardType.Money, _levelScore.ResultReward);
            GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, "YandexSDK", $"Yandex");
        }
    }
}