using System;
using Achievements;

namespace Parameters
{
    public class CounterParameterLevel
    {
        private int _countSpeedLevel = 1;
        private int _countSizeLevel = 1;
        private int _countIncomeLevel = 1;

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
            _achievementSystem.PassValue(AchievementType.Speed, _countSpeedLevel);
            _countSizeLevel = countSizeLevel;
            _achievementSystem.PassValue(AchievementType.Size, _countSizeLevel);
            _countIncomeLevel = countIncomeLevel;
            _achievementSystem.PassValue(AchievementType.Income, _countIncomeLevel);
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