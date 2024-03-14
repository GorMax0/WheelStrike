using System.Collections.Generic;
using UnityEngine;

namespace TemporaryShop
{
    public class TrailShop : MonoBehaviour
    {
        [SerializeField] private TrailView _template;

        private List<TrailView> _views;
        private TrailFX _currentTrail;
        private int _cost;
    }
}