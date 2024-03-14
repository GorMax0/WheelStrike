using Services;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] protected SliderSetter ProgressSlider;
        [SerializeField] protected TextMeshProUGUI TextValue;

        [Header("Stars")] [SerializeField] private Transform _parentStars;
        [SerializeField] private Image _templateStar;
        [SerializeField] private Sprite _fillStar;

        protected int NextValue;
        protected Achievement Achievement;
        protected int CountStars;
        protected Image[] Stars;

        private string _currentName;
        private string _currentDescription;

        protected virtual void OnEnable()
        {
            GetValues();
            FillStars();
            ProgressSlider.SetNormalizedValue(GetNormalizedBarValue());
        }

        private void OnDestroy()
        {
            Localization.LanguageChanged -= OnLocalizationChanged;
        }

        public void Render(Achievement achievement)
        {
            Localization.LanguageChanged += OnLocalizationChanged;
            Achievement = achievement;
            OnLocalizationChanged(Localization.CurrentLanguage);
            _icon.sprite = Achievement.Icon;
            ProgressSlider.Initialize();
            InstantiateStars();
        }

        protected virtual void GetValues()
        {
            NextValue = Achievement.GetNextValueForAchievement();
            TextValue.SetText($"{Achievement.CountValue}/{NextValue}");
        }

        protected virtual float GetNormalizedBarValue()
        {
            if (Achievement.CountValue > NextValue)
            {
                return NextValue / (float)Achievement.CountValue;
            }

            return Achievement.CountValue / (float)NextValue;
        }

        protected virtual void FillStars()
        {
            for (int i = 0; i < CountStars; i++)
            {
                if (i < Achievement.CountAchieved)
                {
                    Stars[i].sprite = _fillStar;
                }
            }
        }

        private void InstantiateStars()
        {
            CountStars = Achievement.GetCountAchievement();
            Stars = new Image[CountStars];

            for (int i = 0; i < CountStars; i++)
            {
                Image star = Instantiate(_templateStar, _parentStars);
                Stars[i] = star;
            }
        }

        private void OnLocalizationChanged(string language)
        {
            switch (language)
            {
                case "Turkish":
                    SetLanguage(0);

                    break;
                case "Russian":
                    SetLanguage(1);

                    break;
                case "English":
                    SetLanguage(2);

                    break;
            }

            _name.text = _currentName;
            _description.text = _currentDescription;
        }

        private void SetLanguage(int index)
        {
            _currentName = Achievement.Name.Entries[index].Text;
            _currentDescription = Achievement.Description.Entries[index].Text;
        }
    }
}