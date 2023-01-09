using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace UI.Views.Money
{
    [RequireComponent(typeof(RectTransform))]
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        private const float ScaleCorrection = 0.6f;
        private const float PositionXMultiplier = 300;
        private readonly float WidthLimiter = Screen.width / 6;

        private RectTransform _rectTransform;
        private Vector3 _startPosition;
        private Color _startColor = new Color(1, 1, 1, 1);
        private float _maxRandomOffset = 80;
        private float _movingDistanceY = 250f;
        private float _tweenMoveDuration = 0.5f;
        private float _tweenFadeDuration = 0.20f;
        private float _endTransparency = 0f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void SetTransformParameters(Vector3 position)
        {
            float newPositionX = Mathf.Clamp(position.x * PositionXMultiplier, -WidthLimiter, WidthLimiter);
            _startPosition = new Vector3(newPositionX, position.y);

            _rectTransform.localPosition = Screen.width < Screen.height ? _startPosition : _startPosition * ScaleCorrection;
            _rectTransform.localScale = Screen.width < Screen.height ? Vector3.one : Vector3.one * ScaleCorrection;
        }

        public void Display(int moneyCount)
        {
            CleanFade();
            _count.text = $"+{moneyCount}";
            gameObject.SetActive(true);
            Animation();
        }

        private void CleanFade()
        {
            _icon.color = _startColor;
            _count.color = _startColor;
        }

        private void Animation()
        {
            float offsetPositionY = Random.Range(-_maxRandomOffset, _maxRandomOffset);
            _movingDistanceY = Screen.width < Screen.height ? _movingDistanceY + offsetPositionY : (_movingDistanceY + offsetPositionY) * ScaleCorrection;

            DOTween.Sequence()
                .Append(_rectTransform.DOAnchorPos(new Vector2(0, _movingDistanceY), _tweenMoveDuration))
                .Append(_icon.DOFade(_endTransparency, _tweenFadeDuration))
                .Join(_count.DOFade(_endTransparency, _tweenFadeDuration))
                .AppendCallback(Disable);
        }

        private void Disable() => gameObject.SetActive(false);
    }
}