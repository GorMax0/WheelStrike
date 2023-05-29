using UnityEngine;

namespace Skins
{
    [CreateAssetMenu(fileName = "NewSkin", menuName = "Gameplay")]
    public class Skin : ScriptableObject
    {
        public GameObject Prefab;
        public Sprite Icon;
        public int Price;
    }
}