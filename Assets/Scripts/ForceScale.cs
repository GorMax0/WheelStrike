using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ForceScale : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField, Range(0.1f, 0.7f)] private float _sliderRange;
    [SerializeField] private float _valueChangeStep;

    private readonly float GreenZoneRaito = 12f;

    private float _value = 1f;
    private Coroutine _changeMultiplier;
    private GameStateService _gameStateService;

    public float Value => _value;

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        _slider.maxValue = Mathf.Max(_sliderRange, -_sliderRange);
        _slider.minValue = Mathf.Min(-_sliderRange, _sliderRange);
    }

    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
        _gameStateService.GameStateChanged += OnGameStateChanged;
    }

    private void StartCoroutine()
    {
        _changeMultiplier = StartCoroutine(ChangeMultiplier());
    }

    private void StopCoroutine()
    {
        if (_changeMultiplier != null)
            StopCoroutine(_changeMultiplier);
    }

    private void RandomizeSliderValue()
    {
        float randomValue = Random.Range(_slider.minValue, _slider.maxValue);
        _slider.value = randomValue;
    }

    //Отделить логику от Слайдера (UI вынести в одтельный скрипт)
    private IEnumerator ChangeMultiplier()
    {
        float endValue = _slider.maxValue;

        while (_gameStateService.State == GameState.Waiting)
        {
            if (_slider.value == endValue && endValue == _slider.maxValue)
                endValue = _slider.minValue;
            else if (_slider.value == endValue && endValue == _slider.minValue)
                endValue = _slider.maxValue;

            _slider.value = Mathf.MoveTowards(_slider.value, endValue, _valueChangeStep);

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
        StopCoroutine();
        StartCoroutine();
    }

    private void OnGameRunning()
    {
        float GreenZoneValue = (Mathf.Abs(_slider.minValue) + _slider.maxValue) / GreenZoneRaito;

        if (_slider.value >= -GreenZoneValue && _slider.value <= GreenZoneValue)
            return;

        _value -= Mathf.Abs(_slider.value);
    }
}
