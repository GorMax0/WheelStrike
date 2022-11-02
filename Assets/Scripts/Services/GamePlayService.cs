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

        public GamePlayService(GameStateService gameStateService, InputHandler inputHandler, Wallet wallet)
        {
            _gameStateService = gameStateService;
            _inputHandler = inputHandler;
            _wallet = wallet;

            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.PointerUp += OnPointerUp;
        }

        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;
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