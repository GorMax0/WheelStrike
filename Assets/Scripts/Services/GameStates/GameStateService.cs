using System;

namespace Services.GameStates
{
    public class GameStateService
    {
        public GameStateService()
        {
            ChangeState(GameState.Initializing);
            ChangeState(GameState.Pause);
        }

        public event Action<GameState> GameStateChanged;

        private GameState _state;        

        public void ChangeState(GameState gameState)
        {
            _state = gameState;
            GameStateChanged?.Invoke(_state);
        }
    }
}