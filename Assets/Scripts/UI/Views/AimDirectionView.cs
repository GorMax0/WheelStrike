using UnityEngine;
using Core;

namespace UI.Views
{
    public class AimDirectionView : MonoBehaviour
    {
        private AimDirection _aimDirection;
        private bool _isInitialize = false;

        private void OnEnable()
        {
            if (_aimDirection == null)
                return;

            _aimDirection.DirectionChanged += OnDirectoinChanged;
        }

        private void OnDisable()
        {
            _aimDirection.DirectionChanged -= OnDirectoinChanged;
        }

        public void Initialize(AimDirection aimDirection)
        {
            if (_isInitialize == true)
                return;

            _aimDirection = aimDirection;
            OnEnable();
            _isInitialize = true;
        }

        private void OnDirectoinChanged(float direction)
        {
            transform.eulerAngles = new Vector3(0, direction, 0);
        }
    }
}