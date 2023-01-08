using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace UI.Views.Money
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _count;

        private const float ScaleCorrection = 0.5625f;

        private RectTransform _rectTransform;
        private float _movingDistanceY = 250f;
        private float _movingDuration = 0.5f;
        private float _intervalDOTween = 0.25f;
        private float _endTransparency = 0f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();

            if(Screen.width > Screen.height)
            _rectTransform.localScale *= ScaleCorrection;
        }

        private void OnEnable()
        {
            _rectTransform.localPosition = Vector3.zero;

            
            Debug.Log($"Multipliy Scale{_rectTransform.localScale}");
            Debug.Log(Screen.safeArea.height/Screen.safeArea.width );
        }

        public void Display(int moneyCount)
        {
            _count.text = $"+{moneyCount}";
            gameObject.SetActive(true);
            Animation();
        }

        private void Disable() => gameObject.SetActive(false);

        private void Animation()
        {
            DOTween.Sequence()
                .Append(_rectTransform.DOAnchorPosY(_movingDistanceY, _movingDuration))
                .AppendInterval(_intervalDOTween)
                .Append(_icon.DOFade(_endTransparency, _movingDuration))
                .Append(_count.DOFade(_endTransparency, _movingDuration))
                .AppendCallback(Disable);
        }
    }
}