using System;
using UnityEngine;
using Core.Wheel;

namespace Trail
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TrailFX : MonoBehaviour
    {
        private ParticleSystem _particle;
        private ParticleSystem.VelocityOverLifetimeModule _velocityOverLifetime;
        private ParticleSystem.MainModule _main;
        private Movement _wheel;
        private float _initialWheelSpeed;
        private float _speedZCorrector;
        private float _lifetimeCorrector;
        private bool _isBought;
        private bool _isSelected = true;

        public bool IsSelected => _isSelected;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
            SetParticleSystemModules();
            SetCorrectionValues();
        }

        private void Update()
        {
            if (_wheel == null)
                return;

            if (HasInitialWheelSpeedNotZero() == false)
                return;

            DecreaseParticleLength();
            AdjustRotationAngle();
        }

        public void SetWheel(Movement wheel)
        {
            if (_wheel != null)
                throw new InvalidOperationException($"{GetType()}: SetWheel(Movement wheel): _wheel already set.");

            _wheel = wheel;
        }

        private void SetParticleSystemModules()
        {
            _velocityOverLifetime = _particle.velocityOverLifetime;
            _main = _particle.main;
        }

        private void SetCorrectionValues()
        {
            _speedZCorrector = _velocityOverLifetime.z.constant;
            _lifetimeCorrector = _main.startLifetime.constant;
        }

        private bool HasInitialWheelSpeedNotZero()
        {
            if (_initialWheelSpeed == 0)
            {
                _initialWheelSpeed = _wheel.Speed;
                return false;
            }

            return true;
        }

        private void DecreaseParticleLength()
        {
            _velocityOverLifetime.z = _wheel.Speed / _initialWheelSpeed * _speedZCorrector;
            _main.startLifetime = _wheel.Speed / _initialWheelSpeed * _lifetimeCorrector;
        }

        private void AdjustRotationAngle()
        {
            if (_wheel.Speed <= 2.1f) //Убрать магическое число, пробросить из константы MinForwardVelocity класса Movement
            {
                _particle.transform.localEulerAngles = new Vector3(-_wheel.transform.eulerAngles.x, 0f, 0f);
            }
        }
    }
}