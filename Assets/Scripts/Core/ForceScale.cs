using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class ForceScale : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 0.7f)] private float _valueRange = 0.18f;
        [SerializeField] private float _duration;

        private const float GreenZoneRaito = 24;

        private float _maxValue;
        private float _minValue;
        private float _currentValue;
        private float _finalValue = 1f;
        private float _startTimerValue = 1f;
        private CoroutineRunning _changeMultiplier;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event UnityAction<float, float> RangeChanged;
        public event UnityAction<float> MultiplierChanged;
        public event UnityAction HitGreenZone;

        public float FinalValue => _finalValue;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService)
        {
            if (_isInitialized == true)
                return;

            _gameStateService = gameStateService;
            _changeMultiplier = new CoroutineRunning(coroutineService);
            SetRange();

            _isInitialized = true;
            OnEnable();
        }

        private void SetRange()
        {
            _minValue = -_valueRange;
            _maxValue = _valueRange;
            RangeChanged?.Invoke(_minValue, _maxValue);
        }

        private IEnumerator ChangeMultiplier()
        {
            float timer = _startTimerValue;

            while (true)
            {
                _currentValue = Mathf.SmoothStep(_minValue, _maxValue, timer);
                timer -= Time.deltaTime / _duration;

                MultiplierChanged?.Invoke(_currentValue);

                if (timer < 0f)
                {
                    _currentValue = _maxValue;
                    _maxValue = _minValue;
                    _minValue = _currentValue;
                    timer = _startTimerValue;
                }

                yield return null;
            }
        }

        private bool TryHitGreenZone()
        {
            float GreenZoneValue = (Mathf.Abs(_minValue) + Mathf.Abs(_maxValue)) / GreenZoneRaito;

            return _currentValue >= -GreenZoneValue && _currentValue <= GreenZoneValue;
        }

        private void CalculateFinalMultiplier() => _finalValue -= Mathf.Abs(_currentValue);

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

        private void OnGameWaiting() => _changeMultiplier.Run(ChangeMultiplier());

        private void OnGameRunning()
        {
            _changeMultiplier.Stop();
            CalculateFinalMultiplier();

            if (TryHitGreenZone() == true)
                HitGreenZone?.Invoke();
        }
    }
}