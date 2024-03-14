using System;

namespace Boost
{
    public class BoostParameter
    {
        public const int MaxLevel = 5;

        public const float BaseSpeedValue = 0.5f;
        public const float BaseIncomeValue = 0.2f;

        public float SpeedMultiplier => BaseSpeedValue * Level;

        public float IncomeMultiplier => BaseIncomeValue * Level;

        public int Level { get; private set; }

        public event Action LevelChanged;

        public void LoadLevel(int level)
        {
            if (level <= 0)
                return;

            Level = level > MaxLevel ? MaxLevel : level;
        }

        public void LevelUp()
        {
            Level++;
            LevelChanged?.Invoke();
        }
    }
}