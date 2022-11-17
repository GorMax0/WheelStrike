using System.Collections;
using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ForceScale))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 20;
        [SerializeField] private float _bounceHeight = 10;

        private const float SpeedDamping = 1.2f;

        private Parameter _speedIncrease;
        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private Rigidbody _rigidbody;
        private AimDirection _aimDirection;
        private InteractionHandler _collisionHandler;
        private Vector3 _offsetAngles;

        private CoroutineRunning _moveForward;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _forceScale = GetComponent<ForceScale>();
            _collisionHandler = GetComponent<InteractionHandler>();
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

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter speedIncrease)
        {
            if (_gameStateService != null || _aimDirection != null)
                return;

            _gameStateService = gameStateService;
            _aimDirection = aimDirection;
            _speedIncrease = speedIncrease;
            _moveForward = new CoroutineRunning(coroutineService);

            OnEnable();
        }

        private void Move()
        {
            float force = _baseSpeed * _speedIncrease.Value * _forceScale.FinalValue;
            Debug.Log(force);

            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * force + _offsetAngles, ForceMode.Acceleration);
        }

        private void RotateInDirection(float directionOffsetX)
        {
            _offsetAngles = new Vector3(0, directionOffsetX, 0);
            transform.eulerAngles = _offsetAngles;
        }

        private IEnumerator HasMoveForward()
        {
            const float MinForwardVelocity = 2.1f;

            while (true)
            {
                yield return new WaitForFixedUpdate();

                if (_rigidbody.velocity.z < MinForwardVelocity)
                    _gameStateService.ChangeState(GameState.Finished);
            }
        }

        private void Bounce()
        {
            float bounceForce = _bounceHeight * _rigidbody.velocity.magnitude;
            _rigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Acceleration);
        }

        private void BounceBack()
        {
            const float MultiplierBackBounce = 0.3f;

            _bounceHeight *= MultiplierBackBounce;
            _rigidbody.AddForce(-Vector3.forward * _bounceHeight, ForceMode.Acceleration);
            Bounce();
            Debug.Log("BounceBack");
        }

        private void CalculateSpeedAfterCollidedWithGround()
        {
            Vector3 speedAfterCollided = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z / SpeedDamping);
            _rigidbody.velocity = speedAfterCollided;
        }

        private void UnfreezeRotation()
        {            
            _rigidbody.freezeRotation = false;          
        }

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

        private void OnCollidedWithGround()
        {
            Bounce();
            CalculateSpeedAfterCollidedWithGround();
        }
    }
}