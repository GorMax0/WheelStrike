using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Parameters;
using DG.Tweening;
using Lean.Localization;

namespace UI.Views
{
    public class ParameterView : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] private Image _label;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _levelUp;
        [SerializeField] private Image _levelUpImage;

        [Header("View for money")]
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Image _coin;
        [SerializeField] private Sprite _arrowDefault;
        [SerializeField] private Sprite _levelUpButtonImageDefault;

        [Header("View for ads")]
        [SerializeField] private Image _arrowRight;
        [SerializeField] private Image _adsIcon;
        [SerializeField] private Sprite _arrowAds;
        [SerializeField] private Sprite _levelUpButtonImageAds;
        [SerializeField] private TMP_Text _adsMultiplierText;

        private const float ScaleRatio = 1.1f;
        private const int InfinityLoops = -1;
        private const float AnimationMoveYDuration = 0.7f;
        private const float AnimationScaleDuration = 0.07f;
        private const float LabelOffsetY = 20f;
        private const string TurkishLanguage = "Turkish";
        private const string RussianLanguage = "Russian";
        private const string EnglishLanguage = "English";
        private const string LevelTurkey = "Seviyesi";
        private const string LevelRussian = "Уровень";
        private const string LevelEnglish = "Level";

        private Parameter _parametr;
        private Vector2 _startScale;
        private Vector2 _scaleIncrease;
        private bool _canBuyingForMoneyState = true;

        public event Action<Parameter, Action> LevelUpForMoneyButtonClicked;
        public event Action<Parameter, Action> LevelUpForAdsButtonClicked;

        public Parameter Parameter => _parametr;

        private void OnEnable()
        {
            LeanLocalization.OnLocalizationChanged += OnLocalizationChanged;
            _levelUp.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            DOTween.Kill(_label.transform);
            LeanLocalization.OnLocalizationChanged -= OnLocalizationChanged;
            _levelUp.onClick.RemoveListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _parametr.Loaded -= Refresh;
        }

        public void Renger(Parameter parametr, int rewardMultiplier)
        {
            _parametr = parametr;
            _name.text = ParameterName.GetName(parametr.Type);
            _level.text = GetLevelText();
            _cost.text = _parametr.Cost.ToString();
            _icon.sprite = _parametr.Icon;
            _adsMultiplierText.text = $"+{rewardMultiplier}";
            _startScale = _levelUp.transform.localScale;
            _scaleIncrease = _startScale * ScaleRatio;

            float animationLabelPosition = _label.transform.localPosition.y + LabelOffsetY;
            _label.transform.DOLocalMoveY(animationLabelPosition, AnimationMoveYDuration).SetLoops(InfinityLoops, LoopType.Yoyo);
        }

        public void SubscribeToLevelChange() => _parametr.Loaded += Refresh;

        public void ChangeStateButton(bool enoughMoney)
        {
            if (_canBuyingForMoneyState == enoughMoney)
                return;

            _cost.gameObject.SetActive(enoughMoney);
            _coin.gameObject.SetActive(enoughMoney);
            _label.sprite = enoughMoney == true ? _arrowDefault : _arrowAds;
            _levelUpImage.sprite = enoughMoney == true ? _levelUpButtonImageDefault : _levelUpButtonImageAds;

            _arrowRight.gameObject.SetActive(!enoughMoney);
            _adsIcon.gameObject.SetActive(!enoughMoney);
            _adsMultiplierText.gameObject.SetActive(!enoughMoney);

            _canBuyingForMoneyState = enoughMoney;
        }

        private void Refresh()
        {
            _level.text = GetLevelText();
            _cost.text = _parametr.Cost.ToString();
        }

        private void OnButtonClick()
        {
            if (_canBuyingForMoneyState == true)
            {
                DOTween.Sequence()
                    .Append(_levelUp.transform.DOScale(_scaleIncrease, AnimationScaleDuration)).SetEase(Ease.InOutQuad)
                    .Append(_levelUp.transform.DOScale(_startScale, AnimationScaleDuration));

                LevelUpForMoneyButtonClicked?.Invoke(_parametr, Refresh);
            }
            else
            {
                LevelUpForAdsButtonClicked?.Invoke(_parametr, Refresh);
            }
        }

        private string GetLevelText()
        {
            string language = LeanLocalization.GetFirstCurrentLanguage();

            switch (language)
            {
                case TurkishLanguage:
                    return $"{LevelTurkey} {_parametr.Level}";

                case RussianLanguage:
                    return $"{LevelRussian} {_parametr.Level}";

                case EnglishLanguage:
                default:
                    return $"{LevelEnglish} {_parametr.Level}";
            }
        }

        private void OnLocalizationChanged()
        {
            _name.text = ParameterName.GetName(_parametr.Type);
            _level.text = GetLevelText();
        }
    }
}