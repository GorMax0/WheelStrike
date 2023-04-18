using System;
using System.Collections.Generic;
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

        public event Action<int> Achieved;

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
            //   Debug.Log("Load.");
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                foreach (AchievementData loadAchievementData in loadAchievementDatasets)
                {
                    //Debug.Log($"Type {loadAchievementData.Type}; Value {loadAchievementData.Value}; IsAchieved {loadAchievementData.IsAchieved}; IsDisplayed {loadAchievementData.IsDisplayed}");
                    if (achievementData.Value == loadAchievementData.Value)
                    {
                        achievementData.IsAchieved = loadAchievementData.IsAchieved;
                        achievementData.IsDisplayed = loadAchievementData.IsDisplayed;
                        loadAchievementDatasets.Remove(loadAchievementData);
                        break;
                    }
                }

                if (achievementData.IsAchieved)
                {
                    _countAchieved++;
                }
            }
        }

        public AchievementData[] SaveValue()
        {
            AchievementData[] saveDatasets = new AchievementData[_achievementDatasets.Length];
            //   Debug.Log("Save.");
            for (int i = 0; i < _achievementDatasets.Length; i++)
            {
                saveDatasets[i] = _achievementDatasets[i];
                //Debug.Log($"Type {saveDatasets[i].Type}; Value {saveDatasets[i].Value}; IsAchieved {saveDatasets[i].IsAchieved}; IsDisplayed {saveDatasets[i].IsDisplayed}");
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
                
                Achieved?.Invoke(_countAchieved);
            }

            CountValue = currentValue;
        }

        public void CheckAchievementValueForTop(int currentValue)
        {
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                if (achievementData.IsAchieved)
                    continue;

                Debug.Log($"{_type} - Current value {currentValue}");

                if (achievementData.HasAchievedForTop(currentValue) == false)
                    return;

                _countAchieved++;

                Debug.Log($"{_type} - Achieved. Count achieved {_countAchieved}");
                Achieved?.Invoke(_countAchieved);
            }

            CountValue = currentValue;
        }
    }
}