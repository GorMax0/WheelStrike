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

namespace Services
{
    public class GamePlayService : IDisposable
    {
        private readonly float IntervalBetweenAds = 140f;
        private readonly float StartDelayHoldTime = 3f;
        private readonly float TimeScaleSlow = 0.1f;
        private readonly float TimeScaleDefault = Time.timeScale;

        private GameStateService _gameStateService;
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
        private Wall _finishWall;
        private float _delayHoldTime;

        public GamePlayService(GameStateService gameStateService, CoroutineService coroutineService, InputHandler inputHandler, InteractionHandler interactionHandler, ITravelable travelable, LevelService levelService, Wallet wallet)
        {
            _gameStateService = gameStateService;
            _coroutineService = coroutineService;
            _inputHandler = inputHandler;
            _interactionHandler = interactionHandler;
            _travelable = travelable;
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _wallet = wallet;
            _holdTime = new CoroutineRunning(_coroutineService);
            _timerForIntervalBetweenAds = new CoroutineRunning(_coroutineService);

            _gameStateService.GameStateChanged += OnGameStateChanged;

            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.PointerUp += OnPointerUp;

            _interactionHandler.CollidedWithObstacle += OnCollidedWithObstacle;
            _interactionHandler.TriggeredEnterWithCar += OnTriggeredEnterWithCar;
            _interactionHandler.TriggeredWithBrick += OnTriggeredWithBrick;
            _interactionHandler.TriggeredWithWall += OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger += OnTriggeredWithCameraTrigger;
        }

        public event Action<Car> TriggeredCar;
        public event Action TimeChangedToDefault;

        public float ElapsedTime { get; private set; }

        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;

            _interactionHandler.CollidedWithObstacle -= OnCollidedWithObstacle;
            _interactionHandler.TriggeredEnterWithCar -= OnTriggeredEnterWithCar;
            _interactionHandler.TriggeredWithBrick -= OnTriggeredWithBrick;
            _interactionHandler.TriggeredWithWall -= OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger -= OnTriggeredWithCameraTrigger;
        }

        public void SetDataOperator(DataOperator dataOperator) => _dataOperator = dataOperator;

        public void SetElapsedTime(float elapsedTime)
        {
            ElapsedTime = elapsedTime <= 0 ? 0 : elapsedTime;
            _timerForIntervalBetweenAds.Run(StartTimerForIntervalBetweenAds());
        }

        private void SetDefaultTime()
        {
            if (Time.timeScale == TimeScaleDefault)
                return;

            Time.timeScale = TimeScaleDefault;
            Time.fixedDeltaTime = Time.fixedUnscaledDeltaTime;
            TimeChangedToDefault?.Invoke();
        }

        private void SetSlowTime()
        {
            if (Time.timeScale == TimeScaleSlow)
                return;

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
                ElapsedTime+=Time.deltaTime;
                yield return null;
            }
        }

        private bool TryShowInterstitialAds()
        {
            if (IntervalBetweenAds - ElapsedTime > 0)
                return false;
      
#if !UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("Show interstitial ads");
#elif YANDEX_GAMES
            Agava.YandexGames.InterstitialAd.Show(OnOpenCallback, OnCloseCallback);
#endif
            ElapsedTime = 0;

            return true;
        }

        private void OnOpenCallback()
        {
            AudioListener.pause = true;
            AudioListener.volume = 0f;
        }

        private void OnCloseCallback(bool isClose)
        {
            AudioListener.pause = !isClose;
            AudioListener.volume = !isClose ? 0f : 1f;
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Finished:
                    OnGameFinished();
                    break;
                case GameState.Restart:
                    OnGameRestart();
                    break;
            }
        }

        private void OnGameFinished()
        {
            _delayHoldTime = 0;
            _finishWall?.StopMoveBricks();
            _wallet.EnrollMoney(_levelScore.ResultScore);
            _levelScore.SetHighscore(_travelable.DistanceTraveled);

            Dispose();
        }

        private void OnGameRestart()
        {
            DOTween.Clear();
            _delayHoldTime = 0;

            TryShowInterstitialAds();
            _dataOperator.Save(); 

            _levelService.RestartLevel();
        }

        private void OnPointerDown() => _gameStateService.ChangeState(GameState.Waiting);

        private void OnPointerUp() => _gameStateService.ChangeState(GameState.Running);

        private void OnCollidedWithObstacle(Obstacle obstacle) => _levelScore.AddScore(obstacle.Reward);

        private void OnTriggeredEnterWithCar(Car car)
        {
            car.Explode();
            car.StopMove();
            _levelScore.AddScore(car.Reward);
            TriggeredCar?.Invoke(car);
            _holdTime.Run(HoldTime());
        }

        private void OnTriggeredWithBrick(Brick brick) => brick.EnableGravity();

        private void OnTriggeredWithWall(Wall wall)
        {
            _finishWall = wall;
            _levelScore.AddScore(wall.Reward);
            wall.PlaySound();
        }

        private void OnTriggeredWithCameraTrigger(CameraTrigger cameraTrigger) => cameraTrigger.OnTriggerEnterWheel();
    }
}