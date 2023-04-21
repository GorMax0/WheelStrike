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

        private Achievement _achievement;
        private string _currentName;
        private string _currentDescription;
        private int _countStars;
        private Image[] _stars;
        
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
            Localization.LanguageChanged  += OnLocalizationChanged;
            _achievement = achievement;
            OnLocalizationChanged(Localization.CurrentLanguage);
            _icon.sprite = _achievement.Icon;
            ProgressSlider.Initialize();
            InstantiateStars();
        }

        protected virtual void GetValues()
        {
            NextValue = _achievement.GetNextValueForAchievement();
            TextValue.SetText($"{_achievement.CountValue}/{NextValue}");
        }
    
        protected virtual float GetNormalizedBarValue() =>
            _achievement.CountValue / (float)NextValue;

        private void InstantiateStars()
        {
            _countStars = _achievement.GetCountAchievement();
           _stars = new Image[_countStars];

            for (int i = 0; i < _countStars; i++)
            {
                var star = Instantiate(_templateStar, _parentStars);
                _stars[i] = star;
            }
        }

        protected void FillStars()
        {
            for (int i = 0; i < _countStars; i++)
            {
                if (i < _achievement.CountAchieved)
                {
                    _stars[i].sprite = _fillStar;
                }
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
            _currentName = _achievement.Name.Entries[index].Text;
            _currentDescription = _achievement.Description.Entries[index].Text;
        }
    }
}