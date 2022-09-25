using UnityEngine;
using Zenject;
using DG.Tweening;
using System;

public class Prerun : MonoBehaviour
{
    [SerializeField] private ForceScaleView _forceScaleView;
    [SerializeField] private AimDirectionView _aimDirectionView;

    private GameStateService _gameStateService;

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
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
            case GameState.Pause:
                OnGamePause();
                break;
            case GameState.Waiting:
                OnGameWaiting();
                break;
            case GameState.Running:
                OnGameRunning();
                break;
        }
    }

    private void OnGamePause()
    {
        _forceScaleView.gameObject.SetActive(false);
        _aimDirectionView.gameObject.SetActive(false);
    }

    private void OnGameWaiting()
    {
        _forceScaleView.gameObject.SetActive(true);
        _aimDirectionView.gameObject.SetActive(true);
        _aimDirectionView.StartTween();
    }

    private void OnGameRunning()
    {
        _aimDirectionView.Fade();
        _forceScaleView.Fade();
    }
}