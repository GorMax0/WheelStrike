using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Achievements
{
    public class AchievementSystem : MonoBehaviour
    {
        [SerializedDictionary(nameof(AchievementType), nameof(Achievement))] [SerializeField]
        private SerializedDictionary<AchievementType, Achievement> _achievements;

        private AchievementQueue _queue;
        private AchievementView _view;
        private int _countAchievement;

        public int TopRank { get; private set; } = 100000;

        private void OnDestroy()
        {
            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                achievement.Value.ValueAchievedChanged -= SumValueAchievedChanged;
            }
        }

        public void Initialize(AchievementQueue queue, AchievementView view)
        {
            _view = view;
            _queue = queue;

            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                achievement.Value.Initialize(achievement.Key);
                achievement.Value.ValueAchievedChanged += SumValueAchievedChanged;
            }

            List<Achievement> achievements = _achievements.Values.ToList();
            _view.Initialize(achievements);
            _queue.Initialize(achievements);
        }

        public void LoadAchievementValue(List<AchievementData> loadDatasets)
        {
            if (loadDatasets == null)
                return;

            foreach (AchievementType achievementKey in _achievements.Keys)
            {
                List<AchievementData> achievementDatasets = new List<AchievementData>();

                foreach (AchievementData loadData in loadDatasets)
                {
                    if (achievementKey == loadData.Type)
                    {
                        achievementDatasets.Add(loadData);
                    }
                }

                _achievements[achievementKey].LoadValue(achievementDatasets);
            }
        }

        public List<AchievementData> Save()
        {
            List<AchievementData> saveDatasets = new List<AchievementData>();

            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                foreach (AchievementData result in achievement.Value.SaveValue())
                {
                    saveDatasets.Add(result);
                }
            }

            return saveDatasets;
        }

        public int SetTopRankValue(int topRank)
        {
            return TopRank = TopRank > topRank ? topRank : TopRank;
        }

        public void PassValue(AchievementType type, int value)
        {
            _achievements[type].CheckAchievementValue(value);
        }

        private void SumValueAchievedChanged(int value)
        {
            _countAchievement -= --value;
            _countAchievement += ++value;
            _view.SetCountAchieved(_countAchievement);
        }
    }
}