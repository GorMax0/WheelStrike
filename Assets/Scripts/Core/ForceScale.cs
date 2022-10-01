using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

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

    public event UnityAction<float> ValueChanged;
    public event UnityAction<float, float> RangeChanged;

    public float FinalValue => _finalValue;

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        SetRange();
    }

    [Inject]
    private void Construct(GameStateService gameStateService, CoroutineService coroutineService)
    {
        _gameStateService = gameStateService;
        _gameStateService.GameStateChanged += OnGameStateChanged;

        _changeMultiplier = new CoroutineRunning(coroutineService);
    }

    private void SetRange()
    {
        _minValue = -_valueRange;
        _maxValue = _valueRange;
        RangeChanged?.Invoke(_minValue, _maxValue);
    }

    private void RandomizeSliderValue()
    {
        float randomValue = Random.Range(_minValue, _maxValue);
        _currentValue = randomValue;
    }

    private IEnumerator ChangeMultiplier()
    {
        float endValue = _maxValue;

        while (_gameStateService.State == GameState.Waiting)
        {
            if (_currentValue == endValue && endValue == _maxValue)
                endValue = _minValue;
            else if (_currentValue == endValue && endValue == _minValue)
                endValue = _maxValue;

            _currentValue = Mathf.MoveTowards(_currentValue, endValue, _valueChangeStep);
            ValueChanged?.Invoke(_currentValue);

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
        RandomizeSliderValue();
        _changeMultiplier.Run(ChangeMultiplier());
    }

    private void OnGameRunning()
    {
        float GreenZoneValue = (Mathf.Abs(_minValue) + _maxValue) / GreenZoneRaito;

        if (_currentValue >= -GreenZoneValue && _currentValue <= GreenZoneValue)
            return;

        _finalValue -= Mathf.Abs(_currentValue);
    }
}
