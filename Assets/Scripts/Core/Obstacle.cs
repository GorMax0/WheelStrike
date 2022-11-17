using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public int Reward { get; private set; }
}
