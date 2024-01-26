using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Core;
using Core.Wheel;
using Data;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;
using GameAnalyticsSDK;
using Authorization;

namespace Services
{
    public class GamePlayService : IDisposable
    {
        private readonly float IntervalBetweenAds = 103f;
        private readonly float StartDelayHoldTime = 3f;
        private readonly float TimeScaleSlow = 0.1f;
        private readonly float TimeScaleDefault = 1f;

        private GameStateService _gameStateService;
        private YandexAuthorization _yandexAuthorization;
        private CoroutineService _coroutineService;
        private InputHandler _inputHandler;
        private InteractionHandler _interactionHandler;
        private ITravelable _travelable;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private Wallet _wallet;
        private DataOperator _dataOperator;
        private CoroutineRunning _holdTime;
        private CoroutineRunning _timerForIntervalBetweenAds;
        private CoroutineRunning _unlockInputHandler;
        private CoroutineRunning _restartLevel;
        private Wall _finishWall;
        private float _delayHoldTime;
        private bool _isRunningAds;

        public GamePlayService(
            GameStateService gameStateService,
            YandexAuthorization yandexAuthorization,
            CoroutineService coroutineService,
            InputHandler inputHandler,
            InteractionHandler interactionHandler,
            ITravelable travelable,
            LevelService levelService,
            Wallet wallet)
        {
            _gameStateService = gameStateService;
            _yandexAuthorization = yandexAuthorization;
            _coroutineService = coroutineService;
            _inputHandler = inputHandler;
            _interactionHandler = interactionHandler;
            _travelable = travelable;
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _wallet = wallet;
            _holdTime = new CoroutineRunning(_coroutineService);
            _timerForIntervalBetweenAds = new CoroutineRunning(_coroutineService);
            _unlockInputHandler = new CoroutineRunning(_coroutineService);
            _restartLevel = new CoroutineRunning(_coroutineService);

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _yandexAuthorization.Authorized += OnAuthorized;

            if (_inputHandler != null)
            {
                _inputHandler.PointerDown += OnPointerDown;
                _inputHandler.PointerUp += OnPointerUp;
            }

            _interactionHandler.CollidedWithObstacle += OnCollidedWithObstacle;
            _interactionHandler.TriggeredEnterWithCar += OnTriggeredEnterWithCar;
            _interactionHandler.TriggeredWithBrick += OnTriggeredWithBrick;
            _interactionHandler.TriggeredWithWall += OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger += OnTriggeredWithCameraTrigger;
            Agava.WebUtility.WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
            CanceledAds += Restart;
        }

        public event Action<Car> TriggeredCar;

        public event Action TimeChangedToDefault;

        public event Action CanceledAds;

        public int Highscore => _levelScore.Highscore;

        public float ElapsedTime { get; private set; }

        public float Playtime { get; private set; }

        public int CountCollisionObstacles { get; private set; }

        public int DistanceTraveledOverAllTime { get; private set; }

        public int CountLaunch { get; private set; }

        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;

            _interactionHandler.CollidedWithObstacle -= OnCollidedWithObstacle;
            _interactionHandler.TriggeredEnterWithCar -= OnTriggeredEnterWithCar;
            _interactionHandler.TriggeredWithBrick -= OnTriggeredWithBrick;
            _interactionHandler.TriggeredWithWall -= OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger -= OnTriggeredWithCameraTrigger;
            Agava.WebUtility.WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
            CanceledAds -= Restart;
        }

        public void SetDataOperator(DataOperator dataOperator) => _dataOperator = dataOperator;

        public void LoadElapsedTime(float elapsedTime)
        {
            ElapsedTime = elapsedTime <= 0 ? 0 : elapsedTime;
            _timerForIntervalBetweenAds.Run(StartTimerForIntervalBetweenAds());
        }

        public void LoadPlaytime(float playtime) => Playtime = playtime;

        public void LoadCountCollisionObstacles(int countCollisionObstacles)
        {
            if (CountCollisionObstacles >= countCollisionObstacles)
                return;

            CountCollisionObstacles = countCollisionObstacles;
        }

        public void LoadDistanceTraveledOverAllTime(int distanceTraveledOverAllTime)
        {
            if (DistanceTraveledOverAllTime >= distanceTraveledOverAllTime)
                return;

            DistanceTraveledOverAllTime = distanceTraveledOverAllTime;
        }

        public void LoadCountLaunch(int count) => CountLaunch = count;

        private IEnumerator UnlockInputHandler()
        {
            yield return new WaitForSeconds(0.25f);

            _inputHandler.gameObject.SetActive(true);
        }

        private void SetDefaultTime()
        {
            Time.timeScale = TimeScaleDefault;
            Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime;
            TimeChangedToDefault?.Invoke();
        }

        private void SetSlowTime()
        {
            Time.timeScale = TimeScaleSlow;
            Time.fixedDeltaTime = Time.timeScale * Time.fixedUnscaledDeltaTime;
        }

        private IEnumerator HoldTime()
        {
            SetSlowTime();

            _delayHoldTime = StartDelayHoldTime * Time.timeScale;

            while (_delayHoldTime > 0)
            {
                _delayHoldTime -= Time.deltaTime;

                yield return null;
            }

            SetDefaultTime();
        }

        private IEnumerator StartTimerForIntervalBetweenAds()
        {
            while (true)
            {
                ElapsedTime += Time.deltaTime;
                Playtime += Time.deltaTime;

                yield return null;
            }
        }

        private bool TryShowInterstitialAds()
        {
            if (IntervalBetweenAds - ElapsedTime > 0)
                return false;

#if !UNITY_WEBGL || UNITY_EDITOR
            ElapsedTime = 0;
            OnGameSave();
#elif YANDEX_GAMES
            Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback, OnErrorCallback, OnOfflineCallback);
#endif
            return true;
        }

        private void ChangePause(bool isPause)
        {
            SoundService.ChangeWhenAd(isPause);
            Time.timeScale = isPause == true ? 0f : TimeScaleDefault;
        }

        private IEnumerator TryRestartLevel()
        {
            yield return new WaitForSeconds(0.3f);

            if (_isRunningAds == false)
                Restart();
        }

        private void Restart()
        {
            _levelService.RestartLevel();
        }

        private void OnOpenCallback()
        {
            GameAnalytics.NewDesignEvent("AdClick:InterstitialAds");
            _isRunningAds = true;
            ChangePause(_isRunningAds);
        }

        private void OnCloseCallback(bool isClose)
        {
            _isRunningAds = false;
            ChangePause(_isRunningAds);
            ElapsedTime = 0;
            OnGameSave();
            CanceledAds?.Invoke();
        }

        private void OnErrorCallback(string error)
        {
            Debug.LogWarning(error);
            OnOfflineCallback();
            CanceledAds?.Invoke();
        }

        private void OnOfflineCallback()
        {
            _isRunningAds = false;
            ChangePause(_isRunningAds);
            CanceledAds?.Invoke();
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
                case GameState.Finished:
                    OnGameFinished();

                    break;
                case GameState.Restart:
                    OnGameRestart();

                    break;
                case GameState.ApplyBoost:
                    OnApplyBoost();

                    break;
                case GameState.Save:
                    OnGameSave();

                    break;
            }
        }

        private void OnGameInitializing() => _unlockInputHandler.Run(UnlockInputHandler());

        private void OnGameWaiting() => GameAnalytics.NewProgressionEvent(
            GAProgressionStatus.Start,
            _levelService.NameForAnalytic);

        private void OnGameFinished()
        {
            _delayHoldTime = 0;
            _finishWall?.StopMoveBricks();
            _wallet.EnrollMoney(_levelScore.ResultReward);
            _levelScore.SetHighscore(_travelable.DistanceTraveled);
            DistanceTraveledOverAllTime += _travelable.DistanceTraveled;
            _levelService.SetNextScene();
            CountLaunch++;
            _gameStateService.ChangeState(GameState.Save);

            GameAnalytics.NewResourceEvent(
                GAResourceFlowType.Source,
                "Money",
                _levelScore.ResultReward,
                "Reward",
                "Finishing");

            GameAnalytics.NewResourceEvent(
                GAResourceFlowType.Source,
                "DistanceTraveled",
                _travelable.DistanceTraveled,
                "Traveled",
                "Finishing");

            if (_travelable.DistanceTraveled >= _levelService.LengthRoad)
                GameAnalytics.NewProgressionEvent(
                    GAProgressionStatus.Complete,
                    _levelService.NameForAnalytic,
                    _travelable.DistanceTraveled);
            else
                GameAnalytics.NewProgressionEvent(
                    GAProgressionStatus.Fail,
                    _levelService.NameForAnalytic,
                    _travelable.DistanceTraveled);

            Dispose();
        }

        private void OnGameRestart()
        {
            DOTween.Clear();
            _delayHoldTime = 0;

            TryShowInterstitialAds();
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _restartLevel.Run(TryRestartLevel());
        }

        private void OnApplyBoost()
        {
            _wallet.Reset();
            _levelService.ResetLevelProgress();
            OnGameSave();
            OnGameRestart();
        }

        private void OnGameSave() => _dataOperator.Save();

        private void OnPointerDown() => _gameStateService.ChangeState(GameState.Waiting);

        private void OnPointerUp() => _gameStateService.ChangeState(GameState.Running);

        private void OnAuthorized() => _gameStateService.ChangeState(GameState.Restart);

        private void OnCollidedWithObstacle(Obstacle obstacle)
        {
            if (obstacle.IsCollided == true)
                return;

            _levelScore.AddReward(obstacle.Reward);
            CountCollisionObstacles++;
        }

        private void OnTriggeredEnterWithCar(Car car)
        {
            car.Explode();
            car.StopMove();
            GameAnalytics.NewDesignEvent("Collision:Car");
            _levelScore.AddReward(car.Reward);
            TriggeredCar?.Invoke(car);
            CountCollisionObstacles++;
            _holdTime.Run(HoldTime());
        }

        private void OnTriggeredWithBrick(Brick brick) => brick.EnableGravity();

        private void OnTriggeredWithWall(Wall wall)
        {
            _finishWall = wall;
            _levelScore.AddReward(wall.Reward);
            wall.Collide();
        }

        private void OnTriggeredWithCameraTrigger(CameraTrigger cameraTrigger) => cameraTrigger.OnTriggerEnterWheel();

        private void OnInBackgroundChange(bool inBackground) =>
            Time.timeScale = inBackground == true ? 0 : TimeScaleDefault;
    }
}