using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class AimDirectionView : MonoBehaviour
{
    [SerializeField] private Image _sliderBackground;
    [SerializeField] private Image _sliderHandler;
    [SerializeField] private float _fadeTime;
    [SerializeField] private float _duration;

    private Slider _slider;
    private Coroutine _coroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void StartTween()
    {
        StopCoroutine();
        StartCoroutine();
    }

    public void Fade()
    {
        _sliderBackground.DOFade(0,_fadeTime);
        _sliderHandler.DOFade(0, _fadeTime);
        Invoke(nameof(Disable), _fadeTime);
    }

    private void StartCoroutine()
    {
        _coroutine = StartCoroutine(Replay());
    }

    private void StopCoroutine()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator Replay()
    {
        float endValue;
        WaitForSeconds waitForSeconds = new WaitForSeconds(_duration);

        while (true)
        {
            endValue = _slider.value == _slider.maxValue ? _slider.minValue : _slider.maxValue;
            _slider.DOValue(endValue, _duration);

            yield return waitForSeconds;
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
