using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using Lean.Localization;
using UnityEngine;

namespace Achievements
{
    [Serializable]
    public class Achievement
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private LeanPhrase _name;
        [SerializeField] private LeanPhrase _description;
        [SerializeField] private AchievementData[] _achievementDatasets;

        private AchievementType _type;
        private int _countAchieved;

        public event Action<int> ValueAchievedChanged;
        public event Action<Achievement, int, int, Action> AchievementChanged;

        public int CountValue { get; private set; }
        public Sprite Icon => _icon;
        public LeanPhrase Name => _name;
        public LeanPhrase Description => _description;
        public int CountAchieved => _countAchieved;
        public AchievementType Type => _type;


        public void Initialize(AchievementType type)
        {
            _type = type;

            for (int i = 0; i < _achievementDatasets.Length; i++)
            {
                _achievementDatasets[i].Type = _type;
            }
        }

        public void LoadValue(List<AchievementData> loadAchievementDatasets)
        {
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                foreach (AchievementData loadAchievementData in loadAchievementDatasets)
                {
                    if (achievementData.Value == loadAchievementData.Value)
                    {
                        achievementData.IsDisplayed = loadAchievementData.IsDisplayed;
                        loadAchievementDatasets.Remove(loadAchievementData);
                        break;
                    }
                }
            }
        }

        public AchievementData[] SaveValue()
        {
            AchievementData[] saveDatasets = new AchievementData[_achievementDatasets.Length];

            for (int i = 0; i < _achievementDatasets.Length; i++)
            {
                saveDatasets[i] = _achievementDatasets[i];
            }

            return saveDatasets;
        }

        public int GetCountAchievement() => _achievementDatasets.Length;

        public int GetNextValueForAchievement()
        {
            foreach (var achievementData in _achievementDatasets)
            {
                if (achievementData.IsAchieved)
                    continue;
                
                return achievementData.Value;
            }

            return _achievementDatasets[^1].Value;
        }

        public void CheckAchievementValue(int currentValue)
        {
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                if (achievementData.IsAchieved)
                    continue;

                if (achievementData.HasAchieved(currentValue) == false)
                    break;

                _countAchieved++;
                ValueAchievedChanged?.Invoke(_countAchieved);

                if (achievementData.IsDisplayed)
                    continue;

                AchievementChanged?.Invoke(this, currentValue, achievementData.Value, HasDisplayed);
            }

            if (_achievementDatasets[^1].Value <= currentValue && _type != AchievementType.Top)
            {
                CountValue = _achievementDatasets[^1].Value;
                return;
            }

            CountValue = currentValue;
        }

        private void HasDisplayed()
        {
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                if (achievementData.IsAchieved && achievementData.IsDisplayed == false)
                {
                    achievementData.IsDisplayed = true;
                }
            }
        }
    }
}