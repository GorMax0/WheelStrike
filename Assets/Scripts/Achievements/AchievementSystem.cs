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

        public int CountAchievement { get; private set; }

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
            Debug.Log("AchievementSystem LoadAchievementValue");
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
                      //  Debug.Log($"AchievementSystem Load Type {loadData.Type}; Value {loadData.Value}; IsAchieved {loadData.IsAchieved}; IsDisplayed {loadData.IsDisplayed}");
                    }
                }

                
                _achievements[achievementKey].LoadValue(achievementDatasets);
            }
        }

        public List<AchievementData> Save()
        {
            Debug.Log("AchievementSystem Save");
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
        
        private void SumValueAchievedChanged(int value)
        {
            CountAchievement -= --value;
            CountAchievement += ++value;
            _view.SetCountAchieved(CountAchievement);
        }
    }
}