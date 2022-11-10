using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "NewCarColor", menuName = "Gameplay/Car Color", order = 52)]
    public class CarColor : ScriptableObject
    {
        [SerializeField] private string _label;
        [SerializeField] private Color _color;
        [SerializeField] private Material _material;
        [SerializeField] private int _basePrice = 50;
        [SerializeField] private bool _isPurchased;

        public string Label => _label;
        public Color Color => _color;
        public Material Material => _material;
        public bool IsPurchased => _isPurchased;
    }
}
