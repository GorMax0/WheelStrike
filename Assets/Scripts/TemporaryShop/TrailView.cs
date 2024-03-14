using System;
using UnityEngine;

namespace TemporaryShop
{
    public class TrailView : MonoBehaviour
    {
        private TrailFX _trail;
        private bool _isSelected;

        public event Action<TrailFX> Selected;
    }
}