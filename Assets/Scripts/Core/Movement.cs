using System;
using System.Linq;
using UnityEngine;
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
            if (_gameStateService == null || _aimDirection == null)
                return;
                
            _gameStateService.GameStateChanged += OnGameStateService;
            _aimDirection.DirectionChanged += RotateInDirection;
            _collisionHandler.CollidedWithGround += OnCollidedWithGround;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _aimDirection.DirectionChanged -= RotateInDirection;
            _collisionHandler.CollidedWithGround -= OnCollidedWithGround;
        }

        public void Initialize(GameStateService gameStateService, AimDirection aimDirection, Parametr[] parametrs)
        {
            if (_gameStateService != null || _aimDirection != null)
                return;

            _gameStateService = gameStateService;
            _aimDirection = aimDirection;

            Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
                ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

            _power = result;
            OnEnable();
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
            if (_rigidbody.velocity.z > 0.5f)  //Сделать константой 0.5f 
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

        private void OnCollidedWithGround()
        {
            Bounce();
        }
    }
}