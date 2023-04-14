using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementView : MonoBehaviour
    {
        [SerializeField] private AchievementElement _template;
        
        private List<Achievement> _achievements;

        public void Initialize(List<Achievement> achievements)
        {
            _achievements = achievements;
        }
    }

    public class AchievementElement : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Slider _progress;
        [SerializeField] private TextMeshProUGUI _textValue;

        [Header("Stars")] 
        [SerializeField] private Transform _parentStars;
        [SerializeField] private Image _templateStar;
        [SerializeField] private Sprite _emptyStar;
        [SerializeField] private Sprite _fillStar;

        private Achievement _achievement;

        public void Render(Achievement achievement)
        {
            
        }
    }
}
