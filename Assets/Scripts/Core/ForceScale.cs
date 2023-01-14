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
        [SerializeField] private ParticleSystem _greenZoneParticle;

        private readonly float GreenZoneRaito = 12;

        private float _maxValue;
        private float _minValue;
        private float _currentValue;
        private float _finalValue = 1f;
        private float _startTimerValue = 1;
        private CoroutineRunning _changeMultiplier;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event UnityAction<float, float> RangeChanged;
        public event UnityAction<float> MultiplierChanged;

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

        private void RandomizeStartValue()
        {
            float randomValue = Random.Range(_minValue, _maxValue);
            _currentValue = randomValue;
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

        private bool HasGreenZone()
        {
            float GreenZoneValue = (Mathf.Abs(_minValue) + _maxValue) / GreenZoneRaito;

            return _currentValue >= -GreenZoneValue && _currentValue <= GreenZoneValue;
        }

        private void PlayEffectGreenZone()
        {
            if (HasGreenZone() == true)
                _greenZoneParticle.Play();
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

        private void OnGameWaiting()
        {
            RandomizeStartValue();
            _changeMultiplier.Run(ChangeMultiplier());
        }

        private void OnGameRunning()
        {
            _changeMultiplier.Stop();
            CalculateFinalMultiplier();
            PlayEffectGreenZone();
        }
    }
}