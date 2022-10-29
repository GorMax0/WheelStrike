using System;

namespace Services.GameStates
{
    public class GameStateService
    {
        public event Action<GameState> GameStateChanged;

        private GameState _state;        

        public void ChangeState(GameState gameState)
        {
            _state = gameState;
            GameStateChanged?.Invoke(_state);
        }
    }
}