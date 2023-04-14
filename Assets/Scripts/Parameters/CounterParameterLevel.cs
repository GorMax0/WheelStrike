using System;
using Achievements;

namespace Parameters
{
    public class CounterParameterLevel
    {
        private int _countSpeedLevel;
        private int _countSizeLevel;
        private int _countIncomeLevel;

        public int CountSpeedLevel => _countSpeedLevel;
        public int CountSizeLevel => _countSizeLevel;
        public int CountIncomeLevel => _countIncomeLevel;

        private readonly AchievementSystem _achievementSystem;

        public CounterParameterLevel(AchievementSystem achievementSystem)
        {
            _achievementSystem = achievementSystem;
        }

        public void Load(int countSpeedLevel, int countSizeLevel, int countIncomeLevel)
        {
            _countSpeedLevel = countSpeedLevel;
            _countSizeLevel = countSizeLevel;
            _countIncomeLevel = countIncomeLevel;
        }
        
        public void CheckAchievement(ParameterType type, int increase = 1)
        {
            switch (type)
            {
                case ParameterType.Speed:
                    _countSpeedLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Speed, _countSpeedLevel);
                    break;
                case ParameterType.Size:
                    _countSizeLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Size, _countSizeLevel);
                    break;
                case ParameterType.Income:
                    _countIncomeLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Income, _countIncomeLevel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}