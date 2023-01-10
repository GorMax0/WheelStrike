using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Services.Coroutines;

namespace UI.Manual
{
    [RequireComponent(typeof(Slider))]
    public class AimManual : MonoBehaviour
    {
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderHandler;
        [SerializeField] private TextEffect _manualText;
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _duration;

        private Slider _slider;
        private CoroutineRunning _replayRunning;

        public void Initialize(CoroutineService coroutineService)
        {
            _slider = GetComponent<Slider>();
            _replayRunning = new CoroutineRunning(coroutineService);
        }

        public void StartTween() => _replayRunning.Run(AnimateHand());

        public void Fade()
        {
            _manualText.DisableLoop();
            _sliderBackground.DOFade(0, _fadeTime);
            _sliderHandler.DOFade(0, _fadeTime);
            Invoke(nameof(Disable), _fadeTime);
        }

        private IEnumerator AnimateHand()
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