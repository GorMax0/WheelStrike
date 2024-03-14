using UnityEngine;

namespace Core
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float _explosionPower;

        private Rigidbody[] _rigidbodies;
        private MeshCollider[] _colliders;
        private readonly Vector3 _additionalVector = new Vector3(0f, 0.7f, 0.4f);
        private readonly int _firstIndex = 1;
        private readonly float _rotationRatio = 1f;
        private bool _isExploded;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<MeshCollider>();
        }

        public void Explode()
        {
            if (_isExploded)
                return;

            _isExploded = true;

            Vector3 epicenter = GetAveragePosition();

            for (int i = _firstIndex; i < _rigidbodies.Length; i++)
            {
                SetForceOfPart(epicenter, i, out Vector3 force);
                RotateOfPart(force, i);
            }

            DisableTriggers();
        }

        private Vector3 GetAveragePosition()
        {
            Vector3 position = Vector3.zero;

            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                position += rigidbody.transform.position;
            }

            position /= _rigidbodies.Length;

            return position;
        }

        private Vector3 CalculateForce(Vector3 epicenter, int partIndex) =>
            (_rigidbodies[partIndex].transform.position - epicenter).normalized * _explosionPower + _additionalVector;

        private void SetForceOfPart(Vector3 epicenter, int partIndex, out Vector3 force)
        {
            force = CalculateForce(epicenter, partIndex);

            _rigidbodies[partIndex].isKinematic = false;
            _rigidbodies[partIndex].AddForce(force, ForceMode.VelocityChange);
        }

        private void RotateOfPart(Vector3 force, int partIndex)
        {
            float randomRatio = Random.Range(-_rotationRatio, _rotationRatio);

            _rigidbodies[partIndex].AddTorque(force * randomRatio, ForceMode.VelocityChange);
        }

        private void DisableTriggers()
        {
            for (int i = _firstIndex; i < _colliders.Length; i++)
            {
                _colliders[i].isTrigger = false;
            }
        }
    }
}