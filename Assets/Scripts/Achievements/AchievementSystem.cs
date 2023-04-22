using System;
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
            Debug.Log("AchievementSystem Initialize");
            foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
            {
                achievement.Value.Initialize(achievement.Key);
                achievement.Value.ValueAchievedChanged += SumValueAchievedChanged;
            }

            _view = view;
            _view.Initialize(_achievements.Values.ToList());
        }

        public void LoadAchievementValue(List<AchievementData> loadDatasets)
        {
            // Debug.Log("AchievementSystem LoadAchievementValue");
            // if (loadDatasets == null)
            //     return;
            //
            // foreach (AchievementType achievementKey in _achievements.Keys)
            // {
            //     List<AchievementData> achievementDatasets = new List<AchievementData>();
            //
            //     foreach (AchievementData loadData in loadDatasets)
            //     {
            //         if (achievementKey == loadData.Type)
            //         {
            //             achievementDatasets.Add(loadData);
            //
            //             if (loadData.IsAchieved)
            //             {
            //                CountAchievement++;
            //                Debug.Log($"{achievementKey}: {CountAchievement}");
            //             }
            //         }
            //     }
            //
            //     
            //     _achievements[achievementKey].LoadValue(achievementDatasets);
            // }
        }

        // public List<AchievementData> Save()
        // {
        //     Debug.Log("AchievementSystem Save");
        //     List<AchievementData> saveDatasets = new List<AchievementData>();
        //
        //     foreach (KeyValuePair<AchievementType, Achievement> achievement in _achievements)
        //     {
        //         foreach (AchievementData result in achievement.Value.SaveValue())
        //         {
        //             saveDatasets.Add(result);
        //         }
        //     }
        //
        //     return saveDatasets;
        // }

        public void PassValue(AchievementType type, int value) => _achievements[type].CheckAchievementValue(value);

        public void PassValueForTop(int value) => _achievements[AchievementType.Top].CheckAchievementValueForTop(value);

        private void SumValueAchievedChanged(int value)
        {
            Debug.Log($"AchievementSystemSumAchieved");
            CountAchievement -= --value;
            CountAchievement += ++value;
            _view.SetCountAchieved(CountAchievement);
        }
    }
}