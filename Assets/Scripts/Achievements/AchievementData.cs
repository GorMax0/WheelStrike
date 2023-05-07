using System;
using UnityEngine;

namespace Achievements
{
    [Serializable]
    public class AchievementData
    {
        [HideInInspector] public AchievementType Type;
        [HideInInspector] public bool IsDisplayed;

        [SerializeField] private int _value;

        public bool IsAchieved { get; private set; }
        public int Value => _value;

        public bool HasAchieved(int currentValue) => IsAchieved = _value <= currentValue;
    }
}