using System;
using Core.Wheel;
using Particles.Trail;
using UnityEngine;

namespace TemporaryShop
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TrailFX : ParticleLength
    {
        private const float MinForwardVelocity = 2.1f;
        private const float InitializeSpeed = 80f;

        private ParticleSystem.MainModule _main;
        private float _lifetimeCorrector;
        // private bool _isBought;

        public bool IsSelected { get; } = true;

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

            if (MovementWheel.Speed > InitializeSpeed)
                return;

            _main.startLifetime = MovementWheel.Speed / InitializeSpeed * _lifetimeCorrector;
        }

        private void AdjustRotationAngle()
        {
            if (MovementWheel.Speed <= MinForwardVelocity)
            {
                Particle.transform.localEulerAngles = new Vector3(-MovementWheel.transform.eulerAngles.x, 0f, 0f);
            }
        }
    }
}