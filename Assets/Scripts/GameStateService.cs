using System;
using Zenject;

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
