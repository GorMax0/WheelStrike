using System;
using System.Collections;
using UnityEngine;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class AimDirection
    {
        private readonly float SwipeSensitivity = 10f;
        private readonly float ClampValue = 1.5f;
        private readonly float TimeCameraBlend;

        private Camera _mainCamera;
        private GameStateService _gameStateService;
        private CoroutineRunning _aimRunning;

        public event Action<float> DirectionChanged;

        public AimDirection(GameStateService gameStateService, CoroutineService coroutineService, float timeCameraBlend)
        {
            _mainCamera = Camera.main;
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            TimeCameraBlend = timeCameraBlend;
            _aimRunning = new CoroutineRunning(coroutineService);
        }

        private void Dispose() => _gameStateService.GameStateChanged -= OnGameStateChanged;

        private IEnumerator SelectDirection(float timeCameraBlend)
        {
            yield return new WaitForSeconds(timeCameraBlend);

            Ray startTouchPosition = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Ray currentTouchPosition;
            float swipeValue;
            float directionOffsetX;

            while (true)
            {
                currentTouchPosition = _mainCamera.ScreenPointToRay(Input.mousePosition);
                swipeValue = (currentTouchPosition.direction.x - startTouchPosition.direction.x) * SwipeSensitivity;
                directionOffsetX = Mathf.Clamp(swipeValue, -ClampValue, ClampValue);
                DirectionChanged?.Invoke(directionOffsetX);

                yield return null;
            }
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

        private void OnGameWaiting() => _aimRunning.Run(SelectDirection(TimeCameraBlend));

        private void OnGameRunning()
        {
            _aimRunning.Stop();
            Dispose();
        }
    }
}