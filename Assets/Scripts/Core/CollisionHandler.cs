using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class CollisionHandler : MonoBehaviour
    {
        public event UnityAction CollidedWithGround;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Ground ground))
            {
                CollidedWithGround?.Invoke();
            }
        }
    }
}