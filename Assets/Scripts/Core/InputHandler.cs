using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameStateService _gameStateService;
    private bool _handleInput = true;

    public event UnityAction PointerDown;
    public event UnityAction PointerUp;

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_handleInput == true)
            PointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_handleInput == true)
            PointerUp?.Invoke();

        _handleInput = false;
    }

    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
        _gameStateService.GameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Initializing:
                break;
            case GameState.Pause:
                break;
            case GameState.Waiting:
                break;
            case GameState.Running:
                break;
            case GameState.Failed:
                break;
            case GameState.Restart:
                break;
            case GameState.Winning:
                break;
        }
    }
}