using System;
using Achievements;

namespace Parameters
{
    public class CounterParameterLevel
    {
        private readonly AchievementSystem _achievementSystem;

        public CounterParameterLevel(AchievementSystem achievementSystem)
        {
            _achievementSystem = achievementSystem;
        }

        public int CountSpeedLevel { get; private set; } = 1;

        public int CountSizeLevel { get; private set; } = 1;

        public int CountIncomeLevel { get; private set; } = 1;

        public void Load(int countSpeedLevel, int countSizeLevel, int countIncomeLevel)
        {
            CountSpeedLevel = countSpeedLevel;
            _achievementSystem.PassValue(AchievementType.Speed, CountSpeedLevel);
            CountSizeLevel = countSizeLevel;
            _achievementSystem.PassValue(AchievementType.Size, CountSizeLevel);
            CountIncomeLevel = countIncomeLevel;
            _achievementSystem.PassValue(AchievementType.Income, CountIncomeLevel);
        }

        public void CheckAchievement(ParameterType type, int increase = 1)
        {
            switch (type)
            {
                case ParameterType.Speed:
                    CountSpeedLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Speed, CountSpeedLevel);

                    break;
                case ParameterType.Size:
                    CountSizeLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Size, CountSizeLevel);

                    break;
                case ParameterType.Income:
                    CountIncomeLevel += increase;
                    _achievementSystem.PassValue(AchievementType.Income, CountIncomeLevel);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}