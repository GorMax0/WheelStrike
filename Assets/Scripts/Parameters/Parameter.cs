using System;
using UnityEngine;

namespace Parameters
{
    public class Parameter
    {
        private const int MaxLevel = 300;

        private float _baseValue;
        private int _baseCost;

        public Parameter(ParameterObject creater)
        {
            Type = creater.Type;
            _baseValue = creater.BaseValue;
            _baseCost = creater.BaseCost;
            Icon = creater.Icon;
        }

        public event Action<Parameter> LevelChanged;
        public event Action Loaded;

        public ParameterType Type { get; }
        public Sprite Icon { get; }
        public int Level { get; private set; } = 1;
        public float Value => _baseValue * Level;
        public int Cost => _baseCost * Level;
        public int MaximumLevel => MaxLevel;

        public void LoadLevel(int level)
        {
            if (level <= 0)
                return;

            Level = level > MaxLevel ? MaxLevel : level;
            Loaded?.Invoke();
        }

        public void LevelUp(int levelCount = 1)
        {
            if (levelCount <= 0)
                return;

            Level += levelCount;
            LevelChanged?.Invoke(this);
        }

        public void Reset()
        {
            Level = 1;
            LevelChanged?.Invoke(this);
        }
    }
}