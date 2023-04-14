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

        private AchievementView _view;
        
        public int CountAchievement { get; private set; }
        
        public void Initialize(AchievementView view)
        {
            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                achievement.Value.Initialize(achievement.Key);
                achievement.Value.Achieved += SumAchieved;
            }
            
            view.Initialize(_achievements.Values.ToList());
        }

        public void LoadCountAchievement(int count) => CountAchievement = count;

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

        public void PassValue(AchievementType type, int value) => _achievements[type].CheckAchievementValue(value);
        
        public void PassValueForTop(int value) => _achievements[AchievementType.Top].CheckAchievementValueForTop(value);

        private void SumAchieved(int value)
        {
            CountAchievement -= --value;
            CountAchievement += ++value;
            PassValue(AchievementType.Achieved, CountAchievement);
        }
    }
}