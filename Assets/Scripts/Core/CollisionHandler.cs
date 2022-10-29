using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class CollisionHandler : MonoBehaviour
    {
        public event UnityAction CollidedWithGround;
        public event UnityAction<int> CollidedWithObstacle;

        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionGround(collision);
            OnCollisionObstacle(collision);
        }

        private void OnCollisionGround(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Ground ground))
            {
                CollidedWithGround?.Invoke();
            }
        }

        private void OnCollisionObstacle(Collision collision)
        {
            if (collision.collider.TryGetComponent(out Obstacle obstacle))
            {
                CollidedWithObstacle?.Invoke(obstacle.Reward);
            }
        }
    }
}