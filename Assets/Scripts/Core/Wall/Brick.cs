using UnityEngine;

namespace Core.Wall
{
    [RequireComponent(typeof(Rigidbody))]
    public class Brick : MonoBehaviour 
    {
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void EnableGravity()
        {
            _rigidbody.useGravity = true;
        }
    }
}