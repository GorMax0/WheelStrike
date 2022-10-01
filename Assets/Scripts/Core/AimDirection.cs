using System;
using System.Collections;
using UnityEngine;
using Zenject;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class AimDirection : IDisposable
    {
        private readonly float SwipeSensitivity = 10f;
        private readonly float ClampValue = 3f;

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
            Ray startTouchPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray currentTouchPosition;
            float swipeValue;
            float directionOffsetX;

            while (_isAiming == true)
            {
                currentTouchPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
                swipeValue = (currentTouchPosition.direction.x - startTouchPosition.direction.x) * SwipeSensitivity;
                directionOffsetX = Mathf.Clamp(swipeValue, -ClampValue, ClampValue);
                DirectionChanged?.Invoke(directionOffsetX);

                yield return null;
            }
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
}