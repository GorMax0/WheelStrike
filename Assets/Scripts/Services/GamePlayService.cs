using System;
using Core;
using Services.GameStates;

namespace Services
{
    public class GamePlayService : IDisposable
    {
        private GameStateService _gameStateService;
        private InputHandler _inputHandler;
        private Wallet _wallet;
        private CollisionHandler _collisionHandler;

        public GamePlayService(GameStateService gameStateService, InputHandler inputHandler, Wallet wallet, CollisionHandler collisionHandler)
        {
            _gameStateService = gameStateService;
            _inputHandler = inputHandler;
            _wallet = wallet;
            _collisionHandler = collisionHandler;

            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.PointerUp += OnPointerUp;
            _collisionHandler.CollidedWithObstacle += AddMoney;
        }


        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;
            _collisionHandler.CollidedWithObstacle -= AddMoney;
        }
        private void AddMoney(int reward)
        {
            _wallet.AddMoney(reward);
        }

        private void OnPointerDown()
        {
            _gameStateService.ChangeState(GameState.Waiting);
        }

        private void OnPointerUp()
        {
            _gameStateService.ChangeState(GameState.Running);
        }
    }
}