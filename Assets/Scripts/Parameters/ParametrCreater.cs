using UnityEngine;

[CreateAssetMenu(fileName = "New Parameter", menuName = "Gameplay/Parameter", order = 51)]
public class ParametrCreater : ScriptableObject
{
    [SerializeField] private ParametrType _type;
    [SerializeField] private float _baseValue = 1.05f;
    [SerializeField] private int _baseCost = 25;
    [SerializeField] private Sprite _icon;

    public ParametrType Type => _type;
    public float BaseValue => _baseValue;
    public int BaseCost => _baseCost;
    public Sprite Icon => _icon;
}
