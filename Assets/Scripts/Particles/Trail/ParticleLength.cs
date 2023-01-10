using UnityEngine;
using Core.Wheel;

public class ParticleLength : MonoBehaviour
{
    [SerializeField] protected Movement MovementWheel;

    protected ParticleSystem Particle;
    protected ParticleSystem.VelocityOverLifetimeModule VelocityOverLifetime;
    protected float InitialWheelSpeed;
    protected float SpeedZCorrector;

    private void Awake()
    {
        Particle = GetComponent<ParticleSystem>();
        SetParticleSystemModules();
        SetCorrectionValues();
    }

    protected virtual void Update()
    {
        if (MovementWheel == null)
            return;

        if (HasInitialWheelSpeedNotZero() == false)
            return;

        DecreaseParticleLength();
    }

    protected virtual void SetParticleSystemModules() => VelocityOverLifetime = Particle.velocityOverLifetime;

    protected virtual void SetCorrectionValues() => SpeedZCorrector = VelocityOverLifetime.z.constant;

    protected virtual void DecreaseParticleLength() => VelocityOverLifetime.z = MovementWheel.Speed / InitialWheelSpeed * SpeedZCorrector;

    private bool HasInitialWheelSpeedNotZero()
    {
        if (InitialWheelSpeed == 0)
        {
            InitialWheelSpeed = MovementWheel.Speed;
            return false;
        }

        return true;
    }
}
