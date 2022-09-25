using System;
using Zenject;

public class GamePlayService : IInitializable, IDisposable
{
    private GameStateService _gameStateService;
    private InputHandler _inputHandler;

    public void Dispose()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
        _inputHandler.PointerDown -= OnPointerDown;
        _inputHandler.PointerUp -= OnPointerUp;
    }

    public void Initialize()
    {
        _gameStateService.GameStateChanged += OnGameStateChanged;
        _inputHandler.PointerDown += OnPointerDown;
        _inputHandler.PointerUp += OnPointerUp;
    }

    [Inject]
    private void Construct(GameStateService gameStateService, InputHandler inputHandler)
    {
        _gameStateService = gameStateService;
        _inputHandler = inputHandler;
    }

    private void OnGameStateChanged(GameState state)
    {
        
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
