using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstacle : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _halfMass;

    [field: SerializeField]  public int Reward { get; private set; }

    public float HalfMass => _halfMass;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _halfMass = _rigidbody.mass / 2f;
    }
}
