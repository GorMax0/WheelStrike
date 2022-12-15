using UnityEngine;

namespace Parameters
{
    public class Parameter
    {
        private float _baseValue;
        private int _baseCost;

        public Parameter(ParameterObject creater)
        {
            Name = ParameretName.GetName(creater.Type);
            _baseValue = creater.BaseValue;
            _baseCost = creater.BaseCost;
            Icon = creater.Icon;
        }

        //  public event Action<int> LevelChanged;
        //  public event Action<int> CostChanged;

        public string Name { get; }
        public Sprite Icon { get; }
        public int Level { get; private set; } = 1;
        public float Value => _baseValue * Level;
        public int Cost => _baseCost * Level;
    }
}