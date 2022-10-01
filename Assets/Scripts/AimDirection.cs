using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class AimDirection :  IDisposable
{
    private GameStateService _gameStateService;
    private CoroutineRunning _aimRunning;
    private bool _isAiming;

    public event Action<float> DirectionChanged;

    public void Dispose()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
    }

    [Inject]
    private void Construct(GameStateService gameStateService, CoroutineService coroutineService)
    {
        _gameStateService = gameStateService;
        _gameStateService.GameStateChanged += OnGameStateChanged;
        _aimRunning = new CoroutineRunning(coroutineService);
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Waiting:
                OnGameWaiting();
                break;
            case GameState.Running:
                OnGameRunning();
                break;
        }
    }

    private IEnumerator SelectDirection()
    {
        Ray ray = default;
        float directionOffsetX;

        while (_isAiming == true)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            directionOffsetX = ray.direction.x;
            DirectionChanged?.Invoke(directionOffsetX);

            yield return null;
        }

        if (ray.origin == Vector3.zero && ray.direction == Vector3.zero)
            throw new ArgumentException($"{typeof(AimDirection)}: SelectDirection():  Ray invalid - ray origin and ray direction equals Vector3.zero.");
    }

    private void OnGameWaiting()
    {
        _isAiming = true;
        _aimRunning.Run(SelectDirection());
    }

    private void OnGameRunning()
    {
        _isAiming = false;
    }
}
