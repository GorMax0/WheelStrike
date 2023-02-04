using UnityEngine;
using UnityEngine.UI;
using Services;
using Lean.Localization;
using GameAnalyticsSDK;

namespace UI.Views
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class SettingPanel : MonoBehaviour
    {
        [SerializeField] private Button _buttonSetting;
        [SerializeField] private Toggle _soundToggle;
        [SerializeField] private Button _englishLanguage;
        [SerializeField] private Button _russianLanguage;
        [SerializeField] private Button _turkeyLanguage;
        [SerializeField] private Toggle _fps;

        private const string TurkishLanguage = "Turkish";
        private const string RussianLanguage = "Russian";
        private const string EnglishLanguage = "English";

        private GridLayoutGroup _gridLayoutGroup;
        private Vector2 _sizeItemPortraitOrientation = new Vector2(100f, 100f);
        private Vector2 _sizeItemLandscapeOrientation = new Vector2(80f, 80f);
        private bool _enabled = false;

        private void Awake()
        {
            _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            _soundToggle.gameObject.SetActive(_enabled);
            _englishLanguage.gameObject.SetActive(_enabled);
            _russianLanguage.gameObject.SetActive(_enabled);
            _turkeyLanguage.gameObject.SetActive(_enabled);
            _fps.gameObject.SetActive(_enabled);
        }

        private void OnEnable()
        {
            ScreenOrientationValidator.Instance.OrientationValidated += OnOrientationValidated;
            _buttonSetting.onClick.AddListener(OnButtonSettingClick);
            _englishLanguage.onClick.AddListener(SwitchOnEnglish);
            _russianLanguage.onClick.AddListener(SwitchOnRussian);
            _turkeyLanguage.onClick.AddListener(SwitchOnTurkish);
        }

        private void OnDisable()
        {
            ScreenOrientationValidator.Instance.OrientationValidated -= OnOrientationValidated;
            _buttonSetting.onClick.RemoveListener(OnButtonSettingClick);
            _englishLanguage.onClick.RemoveListener(SwitchOnEnglish);
            _russianLanguage.onClick.RemoveListener(SwitchOnRussian);
            _turkeyLanguage.onClick.RemoveListener(SwitchOnTurkish);
        }

        private void SwitchOnEnglish()
        {
            LeanLocalization.SetCurrentLanguageAll(EnglishLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:English");
        }

        private void SwitchOnRussian()
        {
            LeanLocalization.SetCurrentLanguageAll(RussianLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:Russian");
        }

        private void SwitchOnTurkish()
        {
            LeanLocalization.SetCurrentLanguageAll(TurkishLanguage);
            GameAnalytics.NewDesignEvent($"guiClick:Language:Turkish");
        }

        private void OnButtonSettingClick()
        {
            _enabled = !_enabled;

            bool enabled = _enabled ? true : false;

            _soundToggle.gameObject.SetActive(enabled);
            _englishLanguage.gameObject.SetActive(enabled);
            _russianLanguage.gameObject.SetActive(enabled);
            _turkeyLanguage.gameObject.SetActive(enabled);
            _fps.gameObject.SetActive(enabled);
        }

        private void OnOrientationValidated(bool isPortraitOrientation)
        {
            _gridLayoutGroup.cellSize = isPortraitOrientation ? _sizeItemPortraitOrientation : _sizeItemLandscapeOrientation;
            _gridLayoutGroup.startAxis = isPortraitOrientation ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
        }
    }
}