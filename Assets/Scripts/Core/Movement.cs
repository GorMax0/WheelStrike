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
      //  [SerializeField] private HingeJoint[] _ropeJoints;
        [SerializeField] private float _speed;
        [SerializeField] private float _turnSpeed;

        private Parametr _power;
        private Rigidbody _rigidbody;
        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private AimDirection _aimDirection;
        private Vector3 _offsetAngles;
        private bool _isMoving;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _aimDirection.DirectionChanged -= RotateInDirection;
        }

        private void FixedUpdate()
        {
            //if (_isMoving == true)
            //{
            //    transform.Rotate(-Vector3.left * Time.deltaTime * _turnSpeed);
            //}
        }

        [Inject]
        private void Construct(GameStateService gameStateService, ForceScale forceScale, AimDirection aimDirection, Parametr[] parametrs)
        {
            _gameStateService = gameStateService;
            _forceScale = forceScale;
            _aimDirection = aimDirection;

            _gameStateService.GameStateChanged += OnGameStateService;
            _aimDirection.DirectionChanged += RotateInDirection;

            Parametr result = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Power)).First()
                ?? throw new NullReferenceException($"{typeof(Movement)}: Construct(Parametr[] parametrs): ParametrType.Power is null.");

            _power = result;
        }

        private void Move()
        {
            float force = _speed * _power.Value * _forceScale.FinalValue;

            _isMoving = true;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * force + _offsetAngles, ForceMode.Acceleration);
        }

        private void RotateInDirection(float directionOffsetX)
        {
            _offsetAngles = new Vector3(0, directionOffsetX, 0);
            transform.eulerAngles = _offsetAngles;
        }

        //private void Untie()
        //{
        //    foreach (HingeJoint joint in _ropeJoints)
        //    {
        //        joint.bra;
        //    }
        //}

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
         //   Untie();
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