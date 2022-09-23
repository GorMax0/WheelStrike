using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;

    private const float SpeedMultiplier = 2f;

    private Rigidbody _rigidbody;
    private bool _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

   private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isMoving == false)
        {
            _isMoving = true;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(Vector3.forward * _speed, ForceMode.Acceleration);
        }
    }

    private void FixedUpdate()
    {
        if (_isMoving == true)
        {
            transform.Rotate(-Vector3.left * Time.deltaTime * _turnSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent(out Ground ground))
        {

        }
    }
}
