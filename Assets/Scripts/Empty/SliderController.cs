using System.Collections;
using Services.Coroutines;
using Services.GameStates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class SliderController : MonoBehaviour
    {
        //[SerializeField] private Slider _slider;
        //[SerializeField] private float _duration;

        //private float _currentValue;
        //private CoroutineRunning _changeMultiplier;
        //private GameStateService _gameStateService;
        //private bool _isInitialized = false;

        //public event UnityAction<float> MultiplierChanged;

        //private void OnEnable()
        //{
        //    if (_isInitialized == false)
        //        return;

        //    _gameStateService.GameStateChanged += OnGameStateChanged;
        //}

        //private void OnDisable()
        //{
        //    _gameStateService.GameStateChanged -= OnGameStateChanged;
        //}

        //public void Initialize(GameStateService gameStateService, CoroutineService coroutineService)
        //{
        //    if (_isInitialized == true)
        //        return;

        //    _gameStateService = gameStateService;
        //    _changeMultiplier = new CoroutineRunning(coroutineService);

        //    _isInitialized = true;
        //    OnEnable();
        //}

        //private IEnumerator ChangeMultiplier()
        //{
        //    float endValue = _maxValue;

        //    while (true)
        //    {
        //        _currentValue = Mathf.MoveTowards(_currentValue, endValue, _valueChangeStep * Time.deltaTime);
        //        MultiplierChanged?.Invoke(_currentValue);

        //        if (_currentValue == _maxValue)
        //            endValue = _minValue;
        //        else if (_currentValue == _minValue)
        //            endValue = _maxValue;

        //        yield return null;
        //    }
        //}

        //private void OnGameStateChanged(GameState state)
        //{
        //    switch (state)
        //    {
        //        case GameState.Waiting:
        //            OnGameWaiting();
        //            break;
        //        case GameState.Running:
        //            OnGameRunning();
        //            break;
        //    }
        //}

        //private void OnGameWaiting()
        //{
        //    _changeMultiplier.Run(ChangeMultiplier());
        //}

        //private void OnGameRunning()
        //{
        //    _changeMultiplier.Stop();
        //}
    }
}