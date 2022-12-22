using System;

namespace Services.GameStates
{
    public class GameStateService
    {
        public event Action<GameState> GameStateChanged;

        private GameState _state;        

        public void ChangeState(GameState gameState)
        {
            if (_state == gameState && _state != 0)
                return;

            _state = gameState;
            GameStateChanged?.Invoke(_state);
        }
    }
}