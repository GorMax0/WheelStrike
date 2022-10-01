using System;
using Zenject;

namespace Services.GameStates
{
    public class GameStateService : IInitializable
    {
        public event Action<GameState> GameStateChanged;

        public GameState State { get; private set; }

        public void Initialize()
        {
            ChangeState(GameState.Initializing);
            ChangeState(GameState.Pause);
        }

        public void ChangeState(GameState gameState)
        {
            State = gameState;
            GameStateChanged?.Invoke(State);
        }
    }
}