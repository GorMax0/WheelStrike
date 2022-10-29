using System;
using System.Collections;
using UnityEngine;
using Cinemachine;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class AimDirection : IDisposable
    {
        private readonly float SwipeSensitivity = 10f;
        private readonly float ClampValue = 1.5f;

        private CinemachineBrain _cinemachine;
        private GameStateService _gameStateService;
        private CoroutineRunning _aimRunning;

        public event Action<float> DirectionChanged;

        public void Dispose()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }
        public AimDirection(GameStateService gameStateService, CoroutineService coroutineService, CinemachineBrain cinemachine)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _cinemachine = cinemachine;
            _aimRunning = new CoroutineRunning(coroutineService);
        }

        private IEnumerator SelectDirection()
        {
            float timeCameraBlend = _cinemachine.m_DefaultBlend.BlendTime;

            yield return new WaitForSeconds(timeCameraBlend);

            Ray startTouchPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            Ray currentTouchPosition;
            float swipeValue;
            float directionOffsetX;

            while (true)
            {
                currentTouchPosition = Camera.main.ScreenPointToRay(Input.mousePosition);
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

        private void OnGameWaiting()
        {
            _aimRunning.Run(SelectDirection());
        }

        private void OnGameRunning()
        {
            _aimRunning.Stop();
        }
    }
}