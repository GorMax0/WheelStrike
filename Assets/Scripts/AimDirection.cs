using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AimDirection : IInitializable, IDisposable
{
    private GameStateService _gameStateService;
    private float _directionOffsetX;

    public void Initialize()
    {
        _gameStateService.GameStateChanged += OnGameStateChanged;
    }

    public void Dispose()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
    }

    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
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

    private Vector3 GetStartPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Vector3 direction = ray.direction;
        Debug.Log(direction.normalized);

        return direction.normalized;
    }

    private void Stop

    private void OnGameWaiting()
    {
      var direction = GetStartPoint();
        _directionOffsetX = direction.x;
        Debug.Log(_directionOffsetX);
    }

    private void OnGameRunning()
    {
        throw new NotImplementedException();
    }


}
