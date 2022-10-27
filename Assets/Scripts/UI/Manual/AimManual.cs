using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using Services.Coroutines;

namespace UI.Manual
{
    [RequireComponent(typeof(Slider))]
    public class AimManual : MonoBehaviour
    {
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderHandler;
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _duration;

        private Slider _slider;
        private CoroutineRunning _replayRunning;


        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        [Inject]
        private void Construct(CoroutineService coroutineService)
        {
            _replayRunning = new CoroutineRunning(coroutineService);
        }

        public void StartTween()
        {
            _replayRunning.Run(Replay());
        }

        public void Fade()
        {
            _sliderBackground.DOFade(0, _fadeTime);
            _sliderHandler.DOFade(0, _fadeTime);
            Invoke(nameof(Disable), _fadeTime);
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
            _replayRunning.Stop();
            gameObject.SetActive(false);
        }
    }
}