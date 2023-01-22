using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

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

        private void SwitchOnEnglish() => LeanLocalization.SetCurrentLanguageAll(EnglishLanguage);

        private void SwitchOnRussian() => LeanLocalization.SetCurrentLanguageAll(RussianLanguage);

        private void SwitchOnTurkish() => LeanLocalization.SetCurrentLanguageAll(TurkishLanguage);

        private void OnButtonSettingClick()
        {
            _soundToggle.gameObject.SetActive(_enabled ? false : true);
            _englishLanguage.gameObject.SetActive(_enabled ? false : true);
            _russianLanguage.gameObject.SetActive(_enabled ? false : true);
            _turkeyLanguage.gameObject.SetActive(_enabled ? false : true);

            _enabled = !_enabled;
        }

        private void OnOrientationValidated(bool isPortraitOrientation)
        {
            _gridLayoutGroup.cellSize = isPortraitOrientation ? _sizeItemPortraitOrientation : _sizeItemLandscapeOrientation;
            _gridLayoutGroup.startAxis = isPortraitOrientation ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
        }
    }
}