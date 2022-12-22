using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class ForceScale : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 0.7f)] private float _valueRange = 0.1f;
        [SerializeField] private float _valueChangeStep;

        private readonly float GreenZoneRaito = 12f;

        private float _maxValue;
        private float _minValue;
        private float _currentValue;
        private float _finalValue = 1f;
        private CoroutineRunning _changeMultiplier;
        private GameStateService _gameStateService;

        public event UnityAction<float, float> RangeChanged;
        public event UnityAction<float> MultiplierChanged;

        public float FinalValue => _finalValue;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            _changeMultiplier = new CoroutineRunning(coroutineService);

            SetRange();
            OnEnable();
        }

        private void SetRange()
        {
            _minValue = -_valueRange;
            _maxValue = _valueRange;
            RangeChanged?.Invoke(_minValue, _maxValue);
        }

        private void RandomizeStartValue()
        {
            float randomValue = Random.Range(_minValue, _maxValue);
            _currentValue = randomValue;
        }

        private IEnumerator ChangeMultiplier()
        {
            float endValue = _maxValue;

            while (true)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, endValue, _valueChangeStep * Time.deltaTime);
                MultiplierChanged?.Invoke(_currentValue);

                if (_currentValue == _maxValue)
                    endValue = _minValue;
                else if (_currentValue == _minValue)
                    endValue = _maxValue;

                yield return null;
            }
        }

        private void CalculateFinalMultiplier()
        {
            float GreenZoneValue = (Mathf.Abs(_minValue) + _maxValue) / GreenZoneRaito;

            if (_currentValue >= -GreenZoneValue && _currentValue <= GreenZoneValue)
                return;

            _finalValue -= Mathf.Abs(_currentValue);
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
            RandomizeStartValue();
            _changeMultiplier.Run(ChangeMultiplier());
        }

        private void OnGameRunning()
        {
            _changeMultiplier.Stop();
            CalculateFinalMultiplier();
        }
    }
}