using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Core;

namespace UI.Views
{
    [RequireComponent(typeof(Slider))]
    public class ForceScaleView : MonoBehaviour
    {
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderHandler;
        [SerializeField] private float _fadeTime;
        [SerializeField] private ForceScale _forceScale;

        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            _forceScale.RangeChanged += OnRangeChanged;
            _forceScale.MultiplierChanged += OnMultiplierChanged;
        }

        private void OnDisable()
        {
            _forceScale.RangeChanged -= OnRangeChanged;
            _forceScale.MultiplierChanged -= OnMultiplierChanged;
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