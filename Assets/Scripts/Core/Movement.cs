using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Parameters;
using Services.GameStates;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private AnimationCurve _deviationWhenSwining;

        private Parametr _power;
        private Rigidbody _rigidbody;
        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private AimDirection _aimDirection;
        private Vector3 _offsetAngles;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateService;
            _forceScale.MultiplierChanged += Swing;
            _aimDirection.DirectionChanged += RotateInDirection;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _forceScale.MultiplierChanged -= Swing;
            _aimDirection.DirectionChanged -= RotateInDirection;
        }

        [Inject]
        private void Construct(GameStateService gameStateService, ForceScale forceScale, AimDirection aimDirection, Parametr[] parametrs)
        {
            _gameStateService = gameStateService;
            _forceScale = forceScale;
            _aimDirection = aimDirection;

            Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
                ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

            _power = result;
        }

        private void Swing(float currentForceValue)
        {
            float swingValue = _deviationWhenSwining.Evaluate(currentForceValue);

            transform.position = new Vector3(transform.position.x, transform.position.y, swingValue);
        }

        private void Move()
        {
            float force = _speed * _power.Value * _forceScale.FinalValue;

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * force + _offsetAngles, ForceMode.Acceleration);
        }

        private void RotateInDirection(float directionOffsetX)
        {
            _offsetAngles = new Vector3(0, directionOffsetX, 0);
            transform.eulerAngles = _offsetAngles;
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
}