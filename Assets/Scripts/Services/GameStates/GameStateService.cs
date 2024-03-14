using System;

namespace Services.GameStates
{
    public class GameStateService
    {
        private GameState _state;

        public event Action<GameState> GameStateChanged;

        public void ChangeState(GameState gameState)
        {
            if (_state != GameState.Save && _state == gameState)
                return;

            _state = gameState;
            GameStateChanged?.Invoke(_state);
        }
    }
}