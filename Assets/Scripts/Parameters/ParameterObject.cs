using UnityEngine;

namespace Parameters
{
    [CreateAssetMenu(fileName = "New Parameter", menuName = "Gameplay/Parameter", order = 51)]
    public class ParameterObject : ScriptableObject
    {
        [SerializeField] private ParameterType _type;
        [SerializeField] private float _baseValue = 1.05f;
        [SerializeField] private int _baseCost = 25;
        [SerializeField] private Sprite _icon;

        public ParameterType Type => _type;
        public float BaseValue => _baseValue;
        public int BaseCost => _baseCost;
        public Sprite Icon => _icon;
    }
}