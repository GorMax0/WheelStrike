using UnityEngine;

namespace Core.Car
{
    public class CarWheel : MonoBehaviour
    {
        private readonly Vector3 RotationAxis = new Vector3(1f, 0f, 0f);
        private readonly float RotationSpeed = 6f;

        private Car _car;
        private bool _isRotate;

        private void Update()
        {
            if (_isRotate == false)
                return;

            transform.Rotate(RotationAxis, RotationSpeed);
        }

        private void OnDestroy()
        {
            _car.IsMovable -= OnIsMovable;
        }

        public void Initialize(Car car)
        {
            _car = car;
            _car.IsMovable += OnIsMovable;
        }

        private void OnIsMovable(bool isMovable) => _isRotate = isMovable;
    }
}