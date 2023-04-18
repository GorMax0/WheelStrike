using System;
using System.Collections.Generic;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementView : MonoBehaviour
    {
        [SerializeField] private AchievementElement _template;
        [SerializeField] private VerticalLayoutGroup _container;
        [SerializeField] private LeanToken _countAchievedText;
        [SerializeField] private LeanToken _sumAchievementText;
        
        private int _countAchievement;
        
        public int CountAchievement { get; private set; }

        private void OnEnable()
        {
            _countAchievedText.Value = SetCountAchieved(CountAchievement).ToString();
        }

        public void Initialize(List<Achievement> achievements)
        {
            foreach (Achievement achievement in achievements)
            {
               AchievementElement viewElement = Instantiate(_template, _container.transform);
               viewElement.Render(achievement);
                _countAchievement += achievement.GetCountAchievement();
            }

            _sumAchievementText.Value = _countAchievement.ToString();
        }

        public int SetCountAchieved(int receivedAchievement) => CountAchievement = receivedAchievement;
    }
}