using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "NewCarColor", menuName = "Gameplay/Car Color", order = 52)]
    public class CarColor : ScriptableObject
    {
        [SerializeField] private Color _color;
        [SerializeField] private Material _material;
        [SerializeField] private bool _isBought;
        [SerializeField] private bool _isSelected;

        public Color Color => _color;
        public Material Material => _material;
        public bool IsBought => _isBought;
        public bool IsSelected => _isSelected;
    }
}
