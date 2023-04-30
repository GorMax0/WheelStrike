using System.Collections.Generic;
using GameAnalyticsSDK;
using Lean.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementView : MonoBehaviour
    {
        [SerializeField] private AchievementElement _template;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private LeanToken _countAchievedText;
        [SerializeField] private LeanToken _sumAchievementText;
        
        private int _countAchieved;
        private int _sumAchievement;

        private void OnEnable()
        {
            _countAchievedText.Value = _countAchieved.ToString();
            _scrollRect.content.anchoredPosition = Vector2.zero;
            GameAnalytics.NewDesignEvent("guiClick:Achievements");
        }

        public void Initialize(List<Achievement> achievements)
        {
            foreach (Achievement achievement in achievements)
            {
                AchievementElement viewElement = Instantiate(_template, _scrollRect.content);
                viewElement.Render(achievement);
                _sumAchievement += achievement.GetCountAchievement();
            }

            _sumAchievementText.Value = _sumAchievement.ToString();
        }

        public void SetCountAchieved(int receivedAchievement) => _countAchieved = receivedAchievement;
    }
}