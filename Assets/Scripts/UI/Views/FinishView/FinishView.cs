using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace UI.Views.Finish
{
    public class FinishView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _topLabel;
        [SerializeField] private TMP_Text _distanceLabel;
        [SerializeField] private Image _rewardBlock;
        [SerializeField] private DistanceBar _distanceBar;

        private Material _uiMaterial;
        private float _alphaOff = 1f;
        private float _endScaleValue = 1f;
        private float _durationFade = 0.5f;
        private float _durationScale = 0.3f;
        private float _intervalBetweenTween = 0.15f;

        private void Awake()
        {
            _uiMaterial = GetComponent<Image>().material;
        }

        public void StartAnimation()
        {
            PrepareView();

            DOTween.Sequence()
               .Append(_uiMaterial.DOFade(_alphaOff, _durationFade).SetEase(Ease.InOutSine))
               .Append(_topLabel.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_distanceLabel.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_rewardBlock.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_distanceBar.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack)); 
        }

        public void ShowDistance(int distance) => _distanceLabel.text = distance.ToString("##" + "m");

        private void PrepareView()
        {
            Color transparentColor = new Color(_uiMaterial.color.r, _uiMaterial.color.g, _uiMaterial.color.b, 0f);
            _uiMaterial.color = transparentColor;

            _topLabel.transform.localScale = Vector3.zero;
            _distanceLabel.transform.localScale = Vector3.zero;
            _rewardBlock.transform.localScale = Vector3.zero;
            _distanceBar.transform.localScale = Vector3.zero;
        }
    }
}