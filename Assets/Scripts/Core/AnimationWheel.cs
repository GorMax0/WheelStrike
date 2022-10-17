using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(ForceScale))]
    public class AnimationWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _deviationWhenSwining;

        private ForceScale _forceScale;

        private void Awake()
        {
            _forceScale = GetComponent<ForceScale>();
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
            float swingValue = _deviationWhenSwining.Evaluate(currentForceValue);

            transform.position = new Vector3(transform.position.x, transform.position.y, swingValue);
        }
    }
}