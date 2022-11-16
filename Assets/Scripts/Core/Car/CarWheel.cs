using System;
using UnityEngine;

namespace Core
{
    public class CarWheel : MonoBehaviour
    {
        private readonly Vector3 RotationAxis = new Vector3(1f, 0f, 0f);
        private readonly float RotationSpeed = 6f;

        private Car _car;
        private bool _isRotate;

        private void OnDestroy()
        {
            _car.IsMovable -= OnIsMovable;
        }

        private void Update()
        {
            if (_isRotate == false)
                return;

            transform.Rotate(RotationAxis, RotationSpeed);
        }

        public void Initialize(Car car)
        {
            _car = car;
            _car.IsMovable += OnIsMovable;
        }

        private void OnIsMovable(bool isMovable) => _isRotate = isMovable;
    }
}