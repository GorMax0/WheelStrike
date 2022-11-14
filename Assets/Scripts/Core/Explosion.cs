using UnityEngine;

namespace Core
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float _explosionPower;

        private bool _isExploded;
        private Rigidbody[] _rigidbodies;
        private MeshCollider[] _colliders;

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

            Vector3 origin = GetAveragePosition();

            for (int i = 0; i < _rigidbodies.Length; i++)
            {
                Vector3 force = (_rigidbodies[i].transform.position - origin).normalized * _explosionPower + Vector3.up;
                _rigidbodies[i].isKinematic = false;

                if (i != 0)
                {
                    _rigidbodies[i].AddForce(force, ForceMode.VelocityChange);
                    _rigidbodies[i].AddTorque(force, ForceMode.VelocityChange);
                }
            }

            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].isTrigger = false;
            }
        }

        private Vector3 GetAveragePosition()
        {
            Vector3 position = Vector3.zero;

            foreach (var rigidbody in _rigidbodies)
            {
                position += rigidbody.transform.position;
            }

            position /= _rigidbodies.Length;
            return position;
        }
    }
}