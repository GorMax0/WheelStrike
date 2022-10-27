using System;
using Core;
using Services.GameStates;

namespace Services
{
    public class GamePlayService : IDisposable
    {
        private GameStateService _gameStateService;
        private InputHandler _inputHandler;

        public void Dispose()
        {
            _inputHandler.PointerDown -= OnPointerDown;
            _inputHandler.PointerUp -= OnPointerUp;
        }

        public GamePlayService(GameStateService gameStateService, InputHandler inputHandler)
        {
            _gameStateService = gameStateService;
            _inputHandler = inputHandler;

            _inputHandler.PointerDown += OnPointerDown;
            _inputHandler.PointerUp += OnPointerUp;
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