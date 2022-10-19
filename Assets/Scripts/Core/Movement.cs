using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Parameters;
using Services.GameStates;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ForceScale))]
    [RequireComponent(typeof(CollisionHandler))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _bounceForce;
        [SerializeField] private float _turnSpeed;
        
        private Parametr _power;
        private Rigidbody _rigidbody;
        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private AimDirection _aimDirection;
        private CollisionHandler _collisionHandler;
        private Vector3 _offsetAngles;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forceScale = GetComponent<ForceScale>();
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateService;
            _aimDirection.DirectionChanged += RotateInDirection;
            _collisionHandler.CollidedWithGround += Bounce;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _aimDirection.DirectionChanged -= RotateInDirection;
            _collisionHandler.CollidedWithGround -= Bounce;
        }

        [Inject]
        private void Construct(GameStateService gameStateService, AimDirection aimDirection, Parametr[] parametrs)
        {
            _gameStateService = gameStateService;
            _aimDirection = aimDirection;

            Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
                ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

            _power = result;
        }

        private void Rotation()
        {
           // _rigidbody.MoveRotation(_turnSpeed);
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

        private void Bounce()
        {
            if (_rigidbody.velocity.z > 0.5f)
                _rigidbody.AddForce(Vector3.up * _bounceForce, ForceMode.Acceleration);
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
    }
}