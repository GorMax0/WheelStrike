using System;
using UnityEngine;
using Core.Wheel;

namespace Trail
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TrailFX : ParticleLength
    {
        private ParticleSystem.MainModule _main;

        private float _lifetimeCorrector;
        private bool _isBought;
        private bool _isSelected = true;

        public bool IsSelected => _isSelected;

        protected override void Update()
        {
            base.Update();
            AdjustRotationAngle();
        }

        public void SetWheel(Movement wheel)
        {
            if (MovementWheel != null)
                throw new InvalidOperationException($"{GetType()}: SetWheel(Movement wheel): _wheel already set.");

            MovementWheel = wheel;
        }

        protected override void SetParticleSystemModules()
        {
            base.SetParticleSystemModules();
            _main = Particle.main;
        }

        protected override void SetCorrectionValues()
        {
            base.SetCorrectionValues();
            _lifetimeCorrector = _main.startLifetime.constant;
        }

        protected override void DecreaseParticleLength()
        {
            base.DecreaseParticleLength();
            _main.startLifetime = MovementWheel.Speed / InitialWheelSpeed * _lifetimeCorrector;
        }

        private void AdjustRotationAngle()
        {
            if (MovementWheel.Speed <= 2.1f) //Убрать магическое число, пробросить из константы MinForwardVelocity класса Movement
            {
                Particle.transform.localEulerAngles = new Vector3(-MovementWheel.transform.eulerAngles.x, 0f, 0f);
            }
        }
    }
}