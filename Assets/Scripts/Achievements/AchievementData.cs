using System;
using UnityEngine;

namespace Achievements
{
    [Serializable]
    public class AchievementData
    {
        [HideInInspector] public AchievementType Type;
        [HideInInspector] public bool IsAchieved;
        [HideInInspector] public bool IsDisplayed;

        [SerializeField] private int _value;

        public int Value => _value;

        public bool HasAchieved(in int currentValue) => IsAchieved = _value <= currentValue;
    }
}