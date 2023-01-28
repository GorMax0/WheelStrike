using UnityEngine;
using Core.Wheel;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public int Reward { get; private set; }
    public bool IsCollided { get; private set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Player wheel))
            IsCollided = true;
    }
}
