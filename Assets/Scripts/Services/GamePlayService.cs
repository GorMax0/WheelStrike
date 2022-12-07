using System;
using Core;
using Core.Wheel;
using Services.GameStates;
using Services.Level;

namespace Services
{
    public class GamePlayService : IDisposable
    {
        private GameStateService _gameStateService;
        private InputHandler _inputHandler;
        private InteractionHandler _interactionHandler;
        private LevelService _levelService;
        private LevelScore _levelScore;
        private Wallet _wallet;

        public GamePlayService(GameStateService gameStateService, InputHandler inputHandler, InteractionHandler interactionHandler, LevelService levelService, Wallet wallet)
        {
            _gameStateService = gameStateService;
            _inputHandler = inputHandler;
            _interactionHandler = interactionHandler;
            _levelService = levelService;
            _levelScore = _levelService.Score;
            _wallet = wallet;

            _gameStateService.GameStateChanged += OnGameStateChanged;

            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.PointerUp += OnPointerUp;

            _interactionHandler.CollidedWithObstacle += OnCollidedWithObstacle;
            _interactionHandler.TriggeredWithCar += OnTriggeredWithCar;
            _interactionHandler.TriggeredWithWall += OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger += OnTriggeredWithCameraTrigger;
        }

        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;

            _interactionHandler.CollidedWithObstacle -= OnCollidedWithObstacle;
            _interactionHandler.TriggeredWithCar -= OnTriggeredWithCar;
            _interactionHandler.TriggeredWithWall -= OnTriggeredWithWall;
            _interactionHandler.TriggeredWithCameraTrigger -= OnTriggeredWithCameraTrigger;
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
            _wallet.EnrollMoney(_levelScore.ResultScore);
            Dispose();
        }

        private void OnGameRestart()
        {
            _levelService.RestartLevel();
        }

        private void OnPointerDown()
        {
            _gameStateService.ChangeState(GameState.Waiting);
        }

        private void OnPointerUp()
        {
            _gameStateService.ChangeState(GameState.Running);
        }

        private void OnCollidedWithObstacle(Obstacle obstacle)
        {
            _levelScore.AddScore(obstacle.Reward);
        }

        private void OnTriggeredWithCar(Car car)
        {
            car.Explode();
            car.StopMove();
            _levelScore.AddScore(car.Reward);
            //  UnityEngine.Time.timeScale = 0.1f;
        }

        private void OnTriggeredWithWall(Wall wall)
        {
            _levelScore.AddScore(wall.Reward);
            wall.EnableGravityBricks();
        }

        private void OnTriggeredWithCameraTrigger(CameraTrigger cameraTrigger)
        {
            cameraTrigger.OnTriggerEnterWheel();
        }
    }
}