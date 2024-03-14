using System;
using System.Collections;
using Services.Coroutines;
using Services.GameStates;
using UnityEngine;

namespace AdsReward
{
    public class RewardScaler
    {
        private const int RewardRateX2 = 2;
        private const int RewardRateX3 = 3;
        private const int RewardRateX5 = 5;
        private const float StartTimerValue = 1;
        private const float Duration = 0.9f;
        private const float RedField = 0.7f;
        private const float YellowField = 0.27f;
        private float _currentValue;
        private RewardZone _currentZone = RewardZone.MiddleX5;

        private readonly GameStateService _gameStateService;
        private float _maxValue = 1;
        private float _minValue = -1;
        private readonly CoroutineRunning _replayRunning;

        public RewardScaler(GameStateService gameStateService, CoroutineService coroutineService)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;

            _replayRunning = new CoroutineRunning(coroutineService);
        }

        public int CurrentRate { get; private set; }

        public event Action<RewardZone> ZoneTransmitted;

        public event Action<float> CurrentValueChanged;

        public void StopTween() => _replayRunning.Stop();

        private IEnumerator ChangeMultiplier()
        {
            float timer = StartTimerValue;

            while (true)
            {
                _currentValue = Mathf.SmoothStep(_maxValue, _minValue, timer);
                timer -= Time.deltaTime / Duration;

                CurrentValueChanged?.Invoke(_currentValue);

                if (timer < 0f)
                {
                    float temp = _maxValue;
                    _maxValue = _minValue;
                    _minValue = temp;
                    timer = StartTimerValue;
                }

                SetZone();

                yield return null;
            }
        }

        private int SetZone()
        {
            switch (_currentValue)
            {
                case <= -RedField when _currentZone == RewardZone.LeftX3:
                    _currentZone = RewardZone.LeftX2;
                    ZoneTransmitted?.Invoke(_currentZone);

                    return CurrentRate = RewardRateX2;
                case >= -RedField and <= -YellowField
                    when _currentZone == RewardZone.MiddleX5 || _currentZone == RewardZone.LeftX2:
                    _currentZone = RewardZone.LeftX3;
                    ZoneTransmitted?.Invoke(_currentZone);

                    return CurrentRate = RewardRateX3;
                case >= -YellowField and <= YellowField
                    when _currentZone == RewardZone.LeftX3 || _currentZone == RewardZone.RightX3:
                    _currentZone = RewardZone.MiddleX5;
                    ZoneTransmitted?.Invoke(_currentZone);

                    return CurrentRate = RewardRateX5;
                case >= YellowField and <= RedField
                    when _currentZone == RewardZone.MiddleX5 || _currentZone == RewardZone.RightX2:
                    _currentZone = RewardZone.RightX3;
                    ZoneTransmitted?.Invoke(_currentZone);

                    return CurrentRate = RewardRateX3;
                case >= RedField when _currentZone == RewardZone.RightX3:
                    _currentZone = RewardZone.RightX2;
                    ZoneTransmitted?.Invoke(_currentZone);

                    return CurrentRate = RewardRateX2;
                default:
                    return CurrentRate;
            }
        }

        private void StartTween() => _replayRunning.Run(ChangeMultiplier());

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.Finished)
            {
                StartTween();
                _gameStateService.GameStateChanged -= OnGameStateChanged;
            }
        }
    }
}