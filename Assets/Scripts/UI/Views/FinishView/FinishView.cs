using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;

namespace UI.Views.Finish
{
    public class FinishView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _topLabel;
        [SerializeField] private TMP_Text _distance;
        [SerializeField] private TMP_Text _highscoreLable;
        [SerializeField] private TMP_Text _score;
        [SerializeField] private TMP_Text _bonusScore;
        [SerializeField] private Image _rewardBlock;
        [SerializeField] private DistanceBar _distanceBar;
        [SerializeField] private RewardScaler _rewardScaler;
        [SerializeField] private Button _playAds;
        [SerializeField] private Button _skipAds;

        private FinishViewHandler _viewHandler;
        private Material _uiMaterial;
        private float _endTransparency = 1f;
        private float _endScaleValue = 1f;
        private float _durationFade = 0.7f;
        private float _durationScale = 0.3f;
        private float _intervalBetweenTween = 0.07f;
        private bool _isInitialized;
        private bool _hasNewHighscore;

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
            _viewHandler.DisplayedHighscoreChanged += OnDispalyNewHighscoreLable;
        }

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedScoreChanged -= OnDisplayedScoreChanged;
            _viewHandler.DisplayedBonusScoreChanged -= OnDisplayedBonusScoreChanged;
            _viewHandler.DisplayedHighscoreChanged -= OnDispalyNewHighscoreLable;
        }

        public void Initialize(FinishViewHandler viewHandler, int lengthRoad)
        {
            _viewHandler = viewHandler;

            InitializeDistanceBar(viewHandler, lengthRoad);
            _isInitialized = true;
            OnEnable();
        }

        public void StartAnimation()
        {
            PrepareView();

            DOTween.Sequence()
               .Append(_uiMaterial.DOFade(_endTransparency, _durationFade).SetEase(Ease.InOutSine))
               .Append(_topLabel.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_distance.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .AppendCallback(_viewHandler.DisplayScore)
               .Append(_rewardBlock.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_distanceBar.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendCallback(_viewHandler.DisplayDistance)
               .AppendCallback(DispalyNewHighscoreLable)
               .AppendCallback(_viewHandler.DisplayBonusScore)
               .AppendInterval(_intervalBetweenTween)
               .Append(_rewardScaler.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_playAds.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack))
               .AppendInterval(_intervalBetweenTween)
               .Append(_skipAds.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack));
        }

        public void Enable() => gameObject.SetActive(true);

        public void Disable() => gameObject.SetActive(false);

        private void InitializeDistanceBar(FinishViewHandler viewHandler, int lengthRoad)
        {
            _distanceBar.Initialize(viewHandler, lengthRoad);
        }

        private void PrepareView()
        {
            Color transparentColor = new Color(_uiMaterial.color.r, _uiMaterial.color.g, _uiMaterial.color.b, 0f);
            _uiMaterial.color = transparentColor;

            _topLabel.transform.localScale = Vector3.zero;
            _distance.transform.localScale = Vector3.zero;
            _highscoreLable.transform.localScale = Vector3.zero;
            _rewardBlock.transform.localScale = Vector3.zero;
            _distanceBar.transform.localScale = Vector3.zero;
            _rewardScaler.transform.localScale = Vector3.zero;
            _playAds.transform.localScale = Vector3.zero;
            _skipAds.transform.localScale = Vector3.zero;
        }

        private void DispalyNewHighscoreLable()
        {
            if (_hasNewHighscore == false)
                return;

            _highscoreLable.transform.DOScale(_endScaleValue, _durationScale).SetEase(Ease.InOutBack);
        }

        private void OnDisplayedDistanceChanged(int distance) => _distance.text = $"{distance}m";

        private void OnDisplayedScoreChanged(int score) => _score.text = $"{score}";

        private void OnDisplayedBonusScoreChanged(int bonusScore) => _bonusScore.text = $"{bonusScore}";

        private void OnDispalyNewHighscoreLable(int newHighscore) => _hasNewHighscore = true;
    }
}