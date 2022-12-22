using System;
using UnityEngine;

namespace Parameters
{
    public class Parameter
    {
        private float _baseValue;
        private int _baseCost;

        public Parameter(ParameterObject creater)
        {
            Type = creater.Type;
            Name = ParameterName.GetName(creater.Type);
            _baseValue = creater.BaseValue;
            _baseCost = creater.BaseCost;
            Icon = creater.Icon;
        }

          public event Action<Parameter> LevelChanged;
        //  public event Action<int> CostChanged;

        public ParameterType Type { get; }
        public string Name { get; }
        public Sprite Icon { get; }
        public int Level { get; private set; } = 1;
        public float Value => _baseValue * Level;
        public int Cost => _baseCost * Level;

        public void LoadLevel(int level)
        {
            if(level <= 0)
                return;

            Level = level;
            LevelChanged?.Invoke(this);
        }

        public void LevelUp()
        {
            Level++;
            LevelChanged?.Invoke(this);
        }
    }
}