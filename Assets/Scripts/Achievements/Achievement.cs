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

        public void CheckAchievementValue(int currentValue)
        {
            foreach (AchievementData achievementData in _achievementDatasets)
            {
                if (achievementData.IsAchieved)
                    continue;

                Debug.Log($"{_type} - Current value {currentValue}");

                if (achievementData.HasAchieved(currentValue) == false)
                    return;

                _countAchieved++;

                Debug.Log($"{_type} - Achieved. Count achieved {_countAchieved}");
                Achieved?.Invoke(_countAchieved);
            }
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
        }
    }
}