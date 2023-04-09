using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Achievements
{
    public class AchievementSystem : MonoBehaviour
    {
        [SerializedDictionary(nameof(AchievementType), nameof(Achievement))] [SerializeField]
        private SerializedDictionary<AchievementType, Achievement> _achievements;

        public void Initialize()
        {
            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                achievement.Value.Initialize(achievement.Key);
            }
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
                        achievementDatasets.Add(loadData);
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
    }
}