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
    private bool _isMoving;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        _gameStateService.GameStateChanged -= OnGameStateService;
    }

    private void FixedUpdate()
    {
        if (_isMoving == true)
        {
            transform.Rotate(-Vector3.left * Time.deltaTime * _turnSpeed);
        }
    }

    [Inject]
    private void Construct(GameStateService gameStateService, ForceScale forceScale, Parametr[] parametrs)
    {
        _gameStateService = gameStateService;
        _gameStateService.GameStateChanged += OnGameStateService;
        _forceScale = forceScale;

        Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
            ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

        _power = result;
    }

    private void Move()
    {
        float force = _speed * _power.Value * _forceScale.Value;

        _isMoving = true;
        _rigidbody.isKinematic = false;
        _rigidbody.AddForce(Vector3.forward * force, ForceMode.Acceleration);
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
