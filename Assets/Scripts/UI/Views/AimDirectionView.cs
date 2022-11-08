using UnityEngine;
using Core;

namespace UI.Views
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AimDirectionView : MonoBehaviour
    {
        private ParticleSystem _aimView;
        private AimDirection _aimDirection;
        private bool _isInitialize = false;

        private void OnEnable()
        {
            if (_aimDirection == null)
                return;

            _aimView.Play();
            _aimDirection.DirectionChanged += OnDirectoinChanged;
        }

        private void OnDisable()
        {
            _aimView.Stop();
            _aimDirection.DirectionChanged -= OnDirectoinChanged;
        }

        public void Initialize(AimDirection aimDirection)
        {
            if (_isInitialize == true)
                return;

            _aimView = GetComponent<ParticleSystem>();
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