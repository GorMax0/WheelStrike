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
        [Header("Common")] [SerializeField] private Image _label;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _levelUp;
        [SerializeField] private Image _levelUpImage;

        [Header("View for money")] [SerializeField]
        private TMP_Text _cost;

        [SerializeField] private Image _coin;
        [SerializeField] private Sprite _arrowDefault;
        [SerializeField] private Sprite _levelUpButtonImageDefault;

        [Header("View for ads")] [SerializeField]
        private Image _arrowRight;

        [SerializeField] private Image _adsIcon;
        [SerializeField] private Sprite _arrowAds;
        [SerializeField] private Sprite _levelUpButtonImageAds;
        [SerializeField] private TMP_Text _adsMultiplierText;

        [Header("View for maximum")] [SerializeField]
        private Sprite _levelUpButtonImageMaximum;

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
        private const float DelayForAds = 2f;

        private Parameter _parameter;
        private Vector2 _startScale;
        private Vector2 _scaleIncrease;
        private ColorBlock _newColorBlock;
        private float _currentDelay;
        private bool _canBuyingForMoneyState = true;

        public event Action<Parameter, Action<bool>> LevelUpForMoneyButtonClicked;
        public event Action<Parameter, Action<bool>> LevelUpForAdsButtonClicked;

        public Parameter Parameter => _parameter;

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
            _parameter.Loaded -= Refresh;
        }

        private void Update()
        {
            if (_levelUp.interactable == true)
                return;
            
            if(_parameter.Level >= _parameter.MaximumLevel)
                return;

            if (DelayForAds > _currentDelay)
            {
                _currentDelay += Time.deltaTime;
                return;
            }

            _levelUp.interactable = true;
            _currentDelay = 0;
        }

        public void Render(Parameter parameter, int rewardMultiplier)
        {
            _parameter = parameter;
            _name.text = ParameterName.GetName(parameter.Type);
            _level.text = GetLevelText();
            _cost.text = _parameter.Cost.ToString();
            _icon.sprite = _parameter.Icon;
            _adsMultiplierText.text = $"+{rewardMultiplier}";
            _startScale = _levelUp.transform.localScale;
            _scaleIncrease = _startScale * ScaleRatio;
            _newColorBlock = _levelUp.colors;

            float animationLabelPosition = _label.transform.localPosition.y + LabelOffsetY;
            _label.transform.DOLocalMoveY(animationLabelPosition, AnimationMoveYDuration).SetLoops(InfinityLoops, LoopType.Yoyo);
        }

        public void SubscribeToLevelChange() => _parameter.Loaded += Refresh;

        public void ChangeStateButton(bool enoughMoney)
        {
            if (_canBuyingForMoneyState == enoughMoney)
                return;

            if (_parameter.Level < _parameter.MaximumLevel)
            {
                _cost.gameObject.SetActive(enoughMoney);
                _coin.gameObject.SetActive(enoughMoney);
                _label.sprite = enoughMoney == true ? _arrowDefault : _arrowAds;
                _levelUpImage.sprite = enoughMoney == true ? _levelUpButtonImageDefault : _levelUpButtonImageAds;

                _arrowRight.gameObject.SetActive(!enoughMoney);
                _adsIcon.gameObject.SetActive(!enoughMoney);
                _adsMultiplierText.gameObject.SetActive(!enoughMoney);

                _levelUp.interactable = false;
                _newColorBlock.disabledColor = new Color(1f, 1f, 1f, 0.7f);
                _levelUp.colors = _newColorBlock;
                
                _canBuyingForMoneyState = enoughMoney;
            }
        }

        private void Refresh(bool isAds)
        {
            _level.text = GetLevelText();

            if (_parameter.Level >= _parameter.MaximumLevel)
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
                _newColorBlock.disabledColor = new Color(1f, 1f, 1f, 1f);
                _levelUp.colors = _newColorBlock;
                return;
            }

            if (isAds == true)
            {
                _levelUp.interactable = false;
                _newColorBlock.disabledColor = new Color(1f, 1f, 1f, 0.7f);
                _levelUp.colors = _newColorBlock;
            }

            _cost.text = _parameter.Cost.ToString();
        }

        private void OnButtonClick()
        {
            if (_canBuyingForMoneyState == true)
            {
                DOTween.Sequence()
                    .Append(_levelUp.transform.DOScale(_scaleIncrease, AnimationScaleDuration)).SetEase(Ease.InOutQuad)
                    .Append(_levelUp.transform.DOScale(_startScale, AnimationScaleDuration));

                LevelUpForMoneyButtonClicked?.Invoke(_parameter, Refresh);
            }
            else
            {
                LevelUpForAdsButtonClicked?.Invoke(_parameter, Refresh);
            }
        }

        private string GetLevelText()
        {
            string language = LeanLocalization.GetFirstCurrentLanguage();

            if (_parameter.Level >= _parameter.MaximumLevel)
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
            else
            {
                switch (language)
                {
                    case TurkishLanguage:
                        return $"{LevelTurkey} {_parameter.Level}";

                    case RussianLanguage:
                        return $"{LevelRussian} {_parameter.Level}";

                    case EnglishLanguage:
                    default:
                        return $"{LevelEnglish} {_parameter.Level}";
                }
            }
        }

        private void OnLocalizationChanged()
        {
            _name.text = ParameterName.GetName(_parameter.Type);
            _level.text = GetLevelText();
        }
    }
}