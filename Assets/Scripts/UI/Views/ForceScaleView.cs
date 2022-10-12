using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;
using Core;

namespace UI.Views
{
    [RequireComponent(typeof(Slider))]
    public class ForceScaleView : MonoBehaviour
    {
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderHandler;
        [SerializeField] private float _fadeTime;

        private Slider _slider;
        private ForceScale _forceScale;

        private void OnDisable()
        {
            _forceScale.RangeChanged -= OnRangeChanged;
            _forceScale.MultiplierChanged -= OnMultiplierChanged;
        }

        [Inject]
        private void Construct(ForceScale forceScale)
        {
            _slider = GetComponent<Slider>();
            _forceScale = forceScale;
            _forceScale.RangeChanged += OnRangeChanged;
            _forceScale.MultiplierChanged += OnMultiplierChanged;
        }

        public void Fade()
        {
            _sliderBackground.DOFade(0, _fadeTime);
            _sliderHandler.DOFade(0, _fadeTime);
            Invoke(nameof(Disable), _fadeTime);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void OnRangeChanged(float minValue, float maxValue)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
        }

        private void OnMultiplierChanged(float value)
        {
            _slider.value = value;
        }
    }
}