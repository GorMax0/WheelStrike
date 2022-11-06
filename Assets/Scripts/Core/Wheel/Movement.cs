using System.Collections;
using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;
using DG.Tweening;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ForceScale))]
    [RequireComponent(typeof(CollisionHandler))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 1800;
        [SerializeField] private float _bounceForce = 200;

        private const float SpeedDamping = 1.5f;

        private Parametr _speedIncrease;
        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private Rigidbody _rigidbody;
        private AimDirection _aimDirection;
        private CollisionHandler _collisionHandler;
        private Vector3 _offsetAngles;

        private CoroutineRunning _moveForward;

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

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parametr speedIncrease)
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
                    _gameStateService.ChangeState(GameState.Failed);
            }
        }

        private void Bounce()
        {
            _rigidbody.AddForce(Vector3.up * _bounceForce, ForceMode.Acceleration);
        }

        private void BounceBack()
        {
            const float MultiplierBackBounce = 0.3f;

            _bounceForce *= MultiplierBackBounce;
            _rigidbody.AddForce(-Vector3.forward * _bounceForce, ForceMode.Acceleration);
            Bounce();
            Debug.Log("BounceBack");
        }

        private void CalculateSpeedAfterCollidedWithGround()
        {
            Vector3 speedAfterCollided = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, _rigidbody.velocity.z - SpeedDamping);
            _rigidbody.velocity = speedAfterCollided;
        }

        private void UnfreezeRotation()
        {            
            _rigidbody.freezeRotation = false;
           // _rigidbody.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
        }

        private void OnGameStateService(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Failed:
                    OnGameFailed();
                    break;
            }
        }

        private void OnGameRunning()
        {
            Move();
            _moveForward.Run(HasMoveForward());
        }

        private void OnGameFailed()
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