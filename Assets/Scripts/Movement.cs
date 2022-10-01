using System;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;

    private Parametr _power;
    private Rigidbody _rigidbody;
    private GameStateService _gameStateService;
    private ForceScale _forceScale;
    private AimDirection _aimDirection;
    private bool _isMoving;
    private Vector3 _test;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateService;
        _aimDirection.DirectionChanged -= RotationInDirection;
    }

    private void FixedUpdate()
    {
        if (_isMoving == true)
        {
            transform.Rotate(-Vector3.left * Time.deltaTime * _turnSpeed);
        }
    }

    [Inject]
    private void Construct(GameStateService gameStateService, ForceScale forceScale, AimDirection aimDirection, Parametr[] parametrs)
    {
        _gameStateService = gameStateService;
        _forceScale = forceScale;
        _aimDirection = aimDirection;

        _gameStateService.GameStateChanged += OnGameStateService;
        _aimDirection.DirectionChanged += RotationInDirection;
        Debug.Log(_aimDirection);

        Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
            ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

        _power = result;
    }

    private void Move()
    {
        float force = _speed * _power.Value * _forceScale.FinalValue;

        _isMoving = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * force, ForceMode.Acceleration);
    }

    private void RotationInDirection(float offsetY)
    {
        _test = new Vector3(0, offsetY, 0)*15;
        transform.eulerAngles = _test;
    }

    private void OnGameStateService(GameState state)
    {
        switch (state)
        {
            case GameState.Running:
                OnGameRunning();
                break;
        }
    }

    private void OnGameRunning()
    {
        Move();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out Ground ground))
        {

        }
    }
}
