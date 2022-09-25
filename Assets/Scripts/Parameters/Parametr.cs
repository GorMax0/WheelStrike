using System;
using UnityEngine;

public class Parametr
{
    private float _baseValue;
    private int _baseCost;

    public event Action<int> LevelChanged;
    public event Action<int> CostChanged;

    public string Name { get; }
    public Sprite Icon { get; }
    public int Level { get; private set; } = 1;
    public float Value { get; private set; }
    public int Cost { get; private set; }

    public Parametr(ParametrCreater creater)
    {
        Name = ParameretName.GetName(creater.Type);
        _baseValue = creater.BaseValue;
        Value = _baseValue;
        _baseCost = creater.BaseCost;
        Cost = _baseCost;
        Icon = creater.Icon;

    }
}
