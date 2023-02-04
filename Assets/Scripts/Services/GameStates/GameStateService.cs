using System;

namespace Services.GameStates
{
    public class GameStateService
    {
        public event Action<GameState> GameStateChanged;

        private GameState _state;        

        public void ChangeState(GameState gameState)
        {
            if (_state != GameState.Save && _state == gameState)
                return;

            _state = gameState;
            GameStateChanged?.Invoke(_state);
        }
    }
}