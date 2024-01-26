using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Empty;
using AdsReward;

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
        [SerializeField] private RewardScalerView _rewardScalerView;
        [SerializeField] private Button _playAds;
        [SerializeField] private TMP_Text _textRewardValue;
        [SerializeField] private Button _skipAds;
        [SerializeField] private ParticleSystem _highscoreEffect;
        [SerializeField] private MoneyViewPresenter _moneyViewPresenter;
        [SerializeField] private SpawnPoint _spawnMoneyPoint;

        private const float EndTransparency = 1f;
        private const float EndScaleValue = 1f;
        private const float DurationFade = 0.7f;
        private const float DurationScale = 0.3f;
        private const float DurationRotate = 0.12f;
        private const float IntervalBetweenTween = 0.07f;
        private const float AdditionalInterval = 0.27f;

        private FinishViewHandler _viewHandler;
        private Material _uiMaterial;
        private Vector3 _angleRotationHighscorLabel = new Vector3(0, 0, 20f);
        private bool _isInitialized;
        private bool _hasNewHighscore;

        public bool IsAction => gameObject.activeInHierarchy;

        private void Awake()
        {
            _uiMaterial = GetComponent<Image>().material;
        }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _rewardScalerView.RewardZoneChanged += OnRewardZoneChanged;

            _viewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
            _viewHandler.DisplayedRewardChanged += OnDisplayedRewardChanged;
            _viewHandler.DisplayedBonusRewardChanged += OnDisplayedBonusRewardChanged;
            _viewHandler.DisplayedHighscoreChanged += OnDisplayNewHighscoreLable;
        }

        private void OnDisable()
        {
            _rewardScalerView.RewardZoneChanged -= OnRewardZoneChanged;

            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedRewardChanged -= OnDisplayedRewardChanged;
            _viewHandler.DisplayedBonusRewardChanged -= OnDisplayedBonusRewardChanged;
            _viewHandler.DisplayedHighscoreChanged -= OnDisplayNewHighscoreLable;
        }

        public void Initialize(FinishViewHandler viewHandler, RewardScaler rewardScaler, int lengthRoad)
        {
            if (_viewHandler == null)
                _viewHandler = viewHandler;

            InitializeDistanceBar(viewHandler, lengthRoad);
            InitializeRewardScalerView(rewardScaler);

            _isInitialized = true;
            OnEnable();
        }

        public void StartAnimation()
        {
            PrepareView();

            DOTween.Sequence()
                .Append(_uiMaterial.DOFade(EndTransparency, DurationFade).SetEase(Ease.InOutSine))
                .Append(_topLabel.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendInterval(IntervalBetweenTween)
                .Append(_distance.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendInterval(IntervalBetweenTween)
                .AppendCallback(_viewHandler.DisplayReward)
                .Append(_rewardBlock.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendInterval(IntervalBetweenTween)
                .Append(_distanceBar.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendCallback(_viewHandler.DisplayDistance)
                .AppendCallback(DisplayNewHighscoreLable)
                .AppendCallback(_viewHandler.DisplayBonusReward)
                .AppendInterval(IntervalBetweenTween)
                .Append(_rewardScalerView.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendInterval(IntervalBetweenTween)
                .Append(_playAds.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack))
                .AppendInterval(IntervalBetweenTween + AdditionalInterval)
                .Append(_skipAds.transform.DOScale(EndScaleValue, DurationScale).ChangeStartValue(Vector3.zero).SetEase(Ease.InOutBack));

            _moneyViewPresenter.RunScatter(_spawnMoneyPoint.transform.localPosition);
        }

        public void Enable()
        {
            _playAds.onClick.AddListener(_viewHandler.OnAdsButtonClick);
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _playAds.onClick.RemoveListener(_viewHandler.OnAdsButtonClick);
        }

        private void InitializeDistanceBar(FinishViewHandler viewHandler, int lengthRoad) => _distanceBar.Initialize(viewHandler, lengthRoad);

        private void InitializeRewardScalerView(RewardScaler rewardScaler) => _rewardScalerView.Initialize(rewardScaler);

        private void PrepareView()
        {
            Color transparentColor = new Color(_uiMaterial.color.r, _uiMaterial.color.g, _uiMaterial.color.b, 0f);
            _uiMaterial.color = transparentColor;
        }

        private void DisplayNewHighscoreLable()
        {
            if (_hasNewHighscore == false)
                return;

            DOTween.Sequence()
                .Append(_highscoreLable.transform.DOScale(EndScaleValue, DurationScale).SetEase(Ease.InOutBack))
                .AppendCallback(PlayEffect)
                .Append(_highscoreLable.transform.DOLocalRotate(_angleRotationHighscorLabel, DurationRotate))
                .Append(_highscoreLable.transform.DOLocalRotate(-_angleRotationHighscorLabel, DurationRotate))
                .Append(_highscoreLable.transform.DOLocalRotate(Vector3.zero, DurationRotate));
        }

        private void PlayEffect() => _highscoreEffect.Play();

        private void OnDisplayedDistanceChanged(int distance) => _distance.text = $"{distance}m";

        private void OnDisplayedRewardChanged(int score) => _score.text = $"{score}";

        private void OnDisplayedBonusRewardChanged(int bonusScore) => _bonusScore.text = $"{bonusScore}";

        private void OnDisplayNewHighscoreLable(int newHighscore) => _hasNewHighscore = true;

        private void OnRewardZoneChanged(string text) => _textRewardValue.text = text;
    }
}