using System.Collections.Generic;
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

        private int _countAchievement;

        public int CountAchievement { get; private set; }

        private void OnEnable()
        {
            _countAchievedText.Value = SetCountAchieved(CountAchievement).ToString();
            _scrollRect.content.anchoredPosition = Vector2.zero;
        }

        public void Initialize(List<Achievement> achievements)
        {
            foreach (Achievement achievement in achievements)
            {
                AchievementElement viewElement = Instantiate(_template, _scrollRect.content);
                viewElement.Render(achievement);
                _countAchievement += achievement.GetCountAchievement();
            }

            _sumAchievementText.Value = _countAchievement.ToString();
        }

        public int SetCountAchieved(int receivedAchievement) => CountAchievement = receivedAchievement;
    }
}