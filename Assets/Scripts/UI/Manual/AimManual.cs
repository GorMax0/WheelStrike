using System.Collections;
using DG.Tweening;
using Services.Coroutines;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Manual
{
    [RequireComponent(typeof(Slider))]
    public class AimManual : MonoBehaviour
    {
        private const int EndValueForTween = 0;
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
            KillTween();
            _manualText.Fade();
            _sliderBackground.DOFade(EndValueForTween, _fadeTime);
            _sliderHandler.DOFade(EndValueForTween, _fadeTime);
            Invoke(nameof(Disable), _fadeTime);
        }

        public void Display()
        {
            _manualText.Prepare();
            _sliderBackground.color = Color.white;
            _sliderHandler.color = Color.white;
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
            _slider.value = _slider.minValue;
            KillTween();
            gameObject.SetActive(false);
        }

        private void KillTween()
        {
            DOTween.Kill(_sliderBackground);
            DOTween.Kill(_sliderHandler);
            DOTween.Kill(_slider);
        }
    }
}