using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SliderSetter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private bool _isAnimated;
        [SerializeField] private float _animationSpeed;
        [SerializeField] private AnimationCurve _curve;

        private float _deltaAnimation;
        private float _endAnimationTime;
        private float _startAnimationValue;
        private float _endAnimationValue;

        private CurveAnimation _curveAnimation;

        private void Awake()
        {
            enabled = false;
            _curveAnimation = new CurveAnimation(_curve, _animationSpeed, () => enabled = false);
        }

        private void Update()
        {
            _slider.SetValueWithoutNotify(_curveAnimation.Update(Time.deltaTime));
        }

        public void SetNormalizedValue(float value)
        {
            Validate(value);
            ApplyValue(value);
        }

        public void SetValueImmediately(float value)
        {
            Validate(value);
            _slider.SetValueWithoutNotify(value);
        }

        private void ApplyValue(float value)
        {
            if (_isAnimated)
            {
                StartAnimation(value);
                return;
            }

            _slider.SetValueWithoutNotify(value);
        }

        private void StartAnimation(float endValue)
        {
            _curveAnimation.StartAnimation(_slider.value, endValue);
            enabled = true;
        }

        private void Validate(float value)
        {
            if (value < _slider.minValue || value > _slider.maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}