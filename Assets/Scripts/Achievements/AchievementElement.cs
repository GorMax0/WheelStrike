using Lean.Localization;
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
        [SerializeField] private SliderSetter _progressSlider;
        [SerializeField] private TextMeshProUGUI _textValue;

        [Header("Stars")] [SerializeField] private Transform _parentStars;
        [SerializeField] private Image _templateStar;
        [SerializeField] private Sprite _fillStar;

        private Achievement _achievement;
        private string _currentName;
        private string _currentDescription;
        private int _nextValue;
        private Image[] _stars;
        private int _countStars;

        private void OnEnable()
        {
            GetValues();
            FillStars();
            _progressSlider.SetNormalizedValue(GetNormalizedBarValue());
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
            _progressSlider.Initialize();
            InstantiateStars();
        }

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

        private void FillStars()
        {
            for (int i = 0; i < _countStars; i++)
            {
                if (i < _achievement.CountAchieved)
                {
                    _stars[i].sprite = _fillStar;
                }
            }
        }

        private void GetValues()
        {
            _nextValue = _achievement.GetNextValueForAchievement();
            _textValue.SetText($"{_achievement.CountValue}/{_nextValue}");
        }
    
        private float GetNormalizedBarValue() =>
            _achievement.CountValue / (float)_nextValue;
        
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