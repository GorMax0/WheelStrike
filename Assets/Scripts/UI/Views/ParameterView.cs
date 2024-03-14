using System;
using DG.Tweening;
using Lean.Localization;
using Parameters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class ParameterView : MonoBehaviour
    {
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
        private const string TurkeyMaximum = "Azami";
        private const string RussianMaximum = "Максимум";
        private const string EnglishMaximum = "Maximum";
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

        [Header("View for maximum")]
        [SerializeField] private Sprite _levelUpButtonImageMaximum;

        private Vector2 _startScale;
        private Vector2 _scaleIncrease;
        private bool _canBuyingForMoneyState = true;

        public event Action<Parameter, Action> LevelUpForMoneyButtonClicked;

        public event Action<Parameter, Action> LevelUpForAdsButtonClicked;

        public Parameter Parameter { get; private set; }

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
            Parameter.Loaded -= Refresh;
        }

        public void Render(Parameter parameter, int rewardMultiplier)
        {
            Parameter = parameter;
            _name.text = ParameterName.GetName(parameter.Type);
            _level.text = GetLevelText();
            _cost.text = Parameter.Cost.ToString();
            _icon.sprite = Parameter.Icon;
            _adsMultiplierText.text = $"+{rewardMultiplier}";
            _startScale = _levelUp.transform.localScale;
            _scaleIncrease = _startScale * ScaleRatio;

            float animationLabelPosition = _label.transform.localPosition.y + LabelOffsetY;

            _label.transform.DOLocalMoveY(animationLabelPosition, AnimationMoveYDuration)
                .SetLoops(InfinityLoops, LoopType.Yoyo);
        }

        public void SubscribeToLevelChange() => Parameter.Loaded += Refresh;

        public void ChangeStateButton(bool enoughMoney)
        {
            if (_canBuyingForMoneyState == enoughMoney)
                return;

            if (Parameter.Level < Parameter.MaximumLevel)
            {
                _cost.gameObject.SetActive(enoughMoney);
                _coin.gameObject.SetActive(enoughMoney);
                _label.sprite = enoughMoney ? _arrowDefault : _arrowAds;
                _levelUpImage.sprite = enoughMoney ? _levelUpButtonImageDefault : _levelUpButtonImageAds;

                _arrowRight.gameObject.SetActive(!enoughMoney);
                _adsIcon.gameObject.SetActive(!enoughMoney);
                _adsMultiplierText.gameObject.SetActive(!enoughMoney);

                _canBuyingForMoneyState = enoughMoney;
            }
        }

        private void Refresh()
        {
            _level.text = GetLevelText();

            if (Parameter.Level >= Parameter.MaximumLevel)
            {
                _levelUp.interactable = false;
                _cost.text = "-";
                _levelUpImage.sprite = _levelUpButtonImageMaximum;
                _label.sprite = _arrowDefault;
                _cost.gameObject.SetActive(true);
                _coin.gameObject.SetActive(true);
                _arrowRight.gameObject.SetActive(false);
                _adsIcon.gameObject.SetActive(false);
                _adsMultiplierText.gameObject.SetActive(false);

                return;
            }

            _cost.text = Parameter.Cost.ToString();
        }

        private void OnButtonClick()
        {
            if (_canBuyingForMoneyState)
            {
                DOTween.Sequence()
                    .Append(_levelUp.transform.DOScale(_scaleIncrease, AnimationScaleDuration))
                    .SetEase(Ease.InOutQuad)
                    .Append(_levelUp.transform.DOScale(_startScale, AnimationScaleDuration));

                LevelUpForMoneyButtonClicked?.Invoke(Parameter, Refresh);
            }
            else
            {
                LevelUpForAdsButtonClicked?.Invoke(Parameter, Refresh);
            }
        }

        private string GetLevelText()
        {
            string language = LeanLocalization.GetFirstCurrentLanguage();

            if (Parameter.Level >= Parameter.MaximumLevel)
            {
                switch (language)
                {
                    case TurkishLanguage:
                        return $"{TurkeyMaximum}";

                    case RussianLanguage:
                        return $"{RussianMaximum}";

                    case EnglishLanguage:
                    default:
                        return $"{EnglishMaximum}";
                }
            }

            switch (language)
            {
                case TurkishLanguage:
                    return $"{LevelTurkey} {Parameter.Level}";

                case RussianLanguage:
                    return $"{LevelRussian} {Parameter.Level}";

                case EnglishLanguage:
                default:
                    return $"{LevelEnglish} {Parameter.Level}";
            }
        }

        private void OnLocalizationChanged()
        {
            _name.text = ParameterName.GetName(Parameter.Type);
            _level.text = GetLevelText();
        }
    }
}