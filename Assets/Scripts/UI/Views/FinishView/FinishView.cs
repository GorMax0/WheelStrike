using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace UI.Views.Finish
{
    public class FinishView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _topLabel;
        [SerializeField] private TMP_Text _distance;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _bonusScore;
        [SerializeField] private Image _rewardBlock;
        [SerializeField] private DistanceBar _distanceBar;

        private FinishViewHandler _viewHandler;
        private Material _uiMaterial;
        private float _alphaOff = 1f;
        private float _endScaleValue = 1f;
        private float _durationFade = 0.5f;
        private float _durationScale = 0.3f;
        private float _intervalBetweenTween = 0.07f;
        private bool _isInitialized;

        public bool IsAction => gameObject.activeInHierarchy == true;

        private void Awake()
        {
            _uiMaterial = GetComponent<Image>().material;
        }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _viewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
            _viewHandler.DisplayedScoreChanged += OnDisplayedScoreChanged;
            _viewHandler.DisplayedBonusScoreChanged += OnDisplayedBonusScoreChanged;
        }

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedScoreChanged -= OnDisplayedScoreChanged;
            _viewHandler.DisplayedBonusScoreChanged -= OnDisplayedBonusScoreChanged;
        }


        public void Initialize(FinishViewHandler finishViewHandler)
        {
            _viewHandler = finishViewHandler;
            _isInitialized = true;
            OnEnable();
        }

        public void StartAnimation()
        {
            PrepareView();

            DOTween.Sequence()
               .Append(_uiMaterial.DOFade(_alphaOff, _durationFade).SetEase(Ease.InOutSine))
               .Append(_topLabel.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_distance.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .AppendCallback(_viewHandler.DisplayDistance)
               .Append(_rewardBlock.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .AppendCallback(_viewHandler.DisplayScore)
               .Append(_distanceBar.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendCallback(_viewHandler.DisplayBonusScore); 
        }

        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        public void OnDisplayedDistanceChanged(int distance) => _distance.text = distance.ToString("#" + "m");
        public void OnDisplayedScoreChanged(int score) => _score.text = score.ToString();
        public void OnDisplayedBonusScoreChanged(int bonusScore) => _bonusScore.text = bonusScore.ToString();

        private void PrepareView()
        {
            Color transparentColor = new Color(_uiMaterial.color.r, _uiMaterial.color.g, _uiMaterial.color.b, 0f);
            _uiMaterial.color = transparentColor;

            _topLabel.transform.localScale = Vector3.zero;
            _distance.transform.localScale = Vector3.zero;
            _rewardBlock.transform.localScale = Vector3.zero;
            _distanceBar.transform.localScale = Vector3.zero;
        }
    }
}