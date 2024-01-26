using System;
using System.Collections;
using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;
using Boost;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ForceScale))]
    public class Movement : MonoBehaviour, ITravelable
    {
        [SerializeField] private float _baseSpeed = 800f;
        [SerializeField] private float _bounceRatio = 0.2f;

        private const int HundredPercent = 1;
        private const float DeviationToSide = 0.135f;
        private const float SpeedDamping = 1.2f;
        private const float DistanceCoefficient = 5f;
        private const float MaximumUpVelocity = 10f;
        private const float LowUpVelocity = 8f;
        private const float MinForwardVelocity = 2.1f;
        private const float MultiplierBackBounce = 0.3f;

        private GameStateService _gameStateService;
        private Parameter _speedIncrease;
        private Parameter _bounceIncrease;
        private BoostParameter _boost;
        private ForceScale _forceScale;
        private Rigidbody _rigidbody;
        private AimDirection _aimDirection;
        private InteractionHandler _collisionHandler;
        private Vector3 _offsetAngles;
        private CoroutineRunning _moveForward;
        private bool _isInitialized = false;

        public int DistanceTraveled => (int)(transform.position.z * DistanceCoefficient);

        public float Speed => _rigidbody.velocity.magnitude;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forceScale = GetComponent<ForceScale>();
            _collisionHandler = GetComponent<InteractionHandler>();
        }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateService;
            _aimDirection.DirectionChanged += OnRotateInDirection;
            _collisionHandler.CollidedWithGround += OnCollidedWithGround;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _aimDirection.DirectionChanged -= OnRotateInDirection;
            _collisionHandler.CollidedWithGround -= OnCollidedWithGround;
        }

        private void FixedUpdate()
        {
            if (_rigidbody.velocity.y >= MaximumUpVelocity)
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, LowUpVelocity, _rigidbody.velocity.z);
        }

        public void Initialize(
            GameStateService gameStateService,
            CoroutineService coroutineService,
            AimDirection aimDirection,
            Parameter speedIncrease,
            Parameter size,
            BoostParameter boost)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException(
                    $"{GetType()}: Initialize(GameStateService gameStateService, "
                    + $"CoroutineService coroutineService, AimDirection aimDirection, Parameter speedIncrease): "
                    + $"Already initialized.");

            _gameStateService = gameStateService;
            _aimDirection = aimDirection;
            _speedIncrease = speedIncrease;
            _bounceIncrease = size;
            _boost = boost;
            _moveForward = new CoroutineRunning(coroutineService);

            _isInitialized = true;
            OnEnable();
        }

        private void Move()
        {
            float minRandomForceRatio = 0.93f;
            float maxRandomForceRatio = 1.03f;
            float randomForceRatio = UnityEngine.Random.Range(minRandomForceRatio, maxRandomForceRatio);
            float boost = HundredPercent + _boost.SpeedMultiplier;
            float force = (_baseSpeed + _speedIncrease.Value * boost) * _forceScale.FinalValue;

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * force * randomForceRatio + _offsetAngles, ForceMode.Acceleration);
        }

        private IEnumerator HasMoveForward()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (_rigidbody.velocity.z < MinForwardVelocity)
                {
                    _rigidbody.velocity = Vector3.zero;
                    _gameStateService.ChangeState(GameState.Finished);
                }
            }
        }

        private void Bounce()
        {
            float deviationToSide = UnityEngine.Random.Range(-DeviationToSide, DeviationToSide);
            float increaseCorrector = 20f;

            float bounceForce = _rigidbody.velocity.magnitude
                * (_bounceRatio + _bounceIncrease.Value / increaseCorrector);

            _rigidbody.AddForce(new Vector3(deviationToSide, bounceForce), ForceMode.VelocityChange);
        }

        private void BounceBack()
        {
            _bounceRatio *= MultiplierBackBounce;
            _rigidbody.AddForce(-Vector3.forward * _bounceRatio, ForceMode.Acceleration);
            Bounce();
        }

        private void CalculateSpeedAfterCollidedWithGround()
        {
            Vector3 speedAfterCollided = new Vector3(
                _rigidbody.velocity.x,
                _rigidbody.velocity.y,
                _rigidbody.velocity.z / SpeedDamping);

            _rigidbody.velocity = speedAfterCollided;
        }

        private void UnfreezeRotation() => _rigidbody.freezeRotation = false;

        private void OnGameStateService(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();

                    break;
                case GameState.Finished:
                    OnGameFinished();

                    break;
            }
        }

        private void OnGameRunning()
        {
            Move();
            _moveForward.Run(HasMoveForward());
        }

        private void OnGameFinished()
        {
            _moveForward.Stop();

            if (_rigidbody.velocity.y != 0)
                BounceBack();

            UnfreezeRotation();
        }

        private void OnRotateInDirection(float directionOffsetX)
        {
            _offsetAngles = new Vector3(0, directionOffsetX, 0);
            transform.eulerAngles = _offsetAngles;
        }

        private void OnCollidedWithGround()
        {
            Bounce();
            CalculateSpeedAfterCollidedWithGround();
        }
    }
}