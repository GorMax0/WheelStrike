using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ForceScale))]
    public class AnimationWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _deviationWhenSwinging;

        private Rigidbody _rigidbody;
        private ForceScale _forceScale;
        private float _startZ;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forceScale = GetComponent<ForceScale>();
            _startZ = transform.position.z;
        }

        private void OnEnable()
        {
            _forceScale.MultiplierChanged += Swing;
        }

        private void OnDisable()
        {
            _forceScale.MultiplierChanged -= Swing;
        }

        private void Swing(float currentForceValue)
        {
            float swingValue = _deviationWhenSwinging.Evaluate(currentForceValue);

            // _rigidbody.MovePosition(new Vector3(transform.position.x, transform.position.y, swingValue));
             transform.position = new Vector3(transform.position.x, transform.position.y, swingValue);
            
        }
    }
}