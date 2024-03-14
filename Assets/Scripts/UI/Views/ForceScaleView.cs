using Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    [RequireComponent(typeof(Slider))]
    public class ForceScaleView : MonoBehaviour
    {
        private const float DurationForTween = 0.4f;
        private const float MaxScaleText = 1f;
        private const float IntervalForTween = 0.2f;
        private const float OffsetMoveY = 170f;
        private const float FadeEndValue = 0f;
        [SerializeField] private Image _sliderBackground;
        [SerializeField] private Image _sliderHandler;
        [SerializeField] private float _fadeTime;
        [SerializeField] private ForceScale _forceScale;
        [SerializeField] private ParticleSystem _greenZoneParticle;
        [SerializeField] private TMP_Text _greenZoneText;

        private Slider _slider;
        private RectTransform _greenZoneTransform;
        private Sequence _doTween;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            _greenZoneTransform = _greenZoneText.GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            _forceScale.RangeChanged += OnRangeChanged;
            _forceScale.MultiplierChanged += OnMultiplierChanged;
            _forceScale.HitGreenZone += OnHitGreenZone;
        }

        private void OnDisable()
        {
            _forceScale.RangeChanged -= OnRangeChanged;
            _forceScale.MultiplierChanged -= OnMultiplierChanged;
            _forceScale.HitGreenZone -= OnHitGreenZone;
        }

        public void Fade()
        {
            _sliderBackground.DOFade(0, _fadeTime);
            _sliderHandler.DOFade(0, _fadeTime);
            Invoke(nameof(Disable), _fadeTime);
        }

        private void Disable()
        {
            _doTween.Kill();
            gameObject.SetActive(false);
        }

        private void AnimateText()
        {
            _greenZoneText.gameObject.SetActive(true);

            _doTween = DOTween.Sequence()
                .Append(
                    _greenZoneText.transform.DOScale(MaxScaleText, DurationForTween)
                        .ChangeStartValue(Vector3.zero)
                        .SetEase(Ease.Flash))
                .AppendInterval(IntervalForTween)
                .Append(_greenZoneTransform.DOAnchorPosY(OffsetMoveY, DurationForTween))
                .Join(_greenZoneText.DOFade(FadeEndValue, DurationForTween));
        }

        private void OnRangeChanged(float minValue, float maxValue)
        {
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
        }

        private void OnMultiplierChanged(float value) => _slider.value = value;

        private void OnHitGreenZone()
        {
            _greenZoneParticle.Play();
            AnimateText();
        }
    }
}