using UnityEngine;
using Services.Coroutines;
using Services.GameStates;
using System.Collections;
using System;

namespace Core.Wheel
{
    [RequireComponent(typeof(ForceScale))]
    [RequireComponent(typeof(InteractionHandler))]
    public class AnimationWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _deviationWhenSwinging;
        [SerializeField] private MeshRenderer _meshForRotation;
        [SerializeField] private float _rotationSpeed;

        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private InteractionHandler _collisionHandler;
        private CoroutineRunning _rotating;
        private CoroutineRunning _figureOfEightRotation;
        private bool _isRotate;
        private bool _isInitialized = false;

        private void Awake()
        {
            _forceScale = GetComponent<ForceScale>();
            _collisionHandler = GetComponent<InteractionHandler>();
        }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateService;
            _forceScale.MultiplierChanged += Swing;
            _collisionHandler.CollidedWithGround += OnCollidedWithGround;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _forceScale.MultiplierChanged -= Swing;
            _collisionHandler.CollidedWithGround -= OnCollidedWithGround;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, CoroutineService coroutineService): Already initialized.");

            _gameStateService = gameStateService;
            _rotating = new CoroutineRunning(coroutineService);
            _figureOfEightRotation = new CoroutineRunning(coroutineService);

            _isInitialized = true;
            OnEnable();
        }

        private void Swing(float currentForceValue)
        {
            float swingValue = _deviationWhenSwinging.Evaluate(currentForceValue);

            transform.position = new Vector3(transform.position.x, transform.position.y, swingValue);
        }

        private IEnumerator Rotate()
        {
            while (true)
            {
                _meshForRotation.transform.Rotate(-Vector3.left * Time.deltaTime * _rotationSpeed);

                yield return null;
            }
        }

        private IEnumerator FigureOfEightRotation()
        {
            float yRotation;
            float zRotation;
            float angleInRadians = 0;
            float jobTime = 0.3f;

            while (jobTime > 0)
            {
                ++angleInRadians;
                zRotation = Mathf.Cos(angleInRadians);
                yRotation = Mathf.Sin(angleInRadians);

                _meshForRotation.transform.Rotate(new Vector3(0f, yRotation, zRotation));
                jobTime -= Time.deltaTime;

                yield return null;
            }

            ResetRotationByYZ();
        }

        private void ResetRotationByYZ()
        {
            _meshForRotation.transform.rotation = new Quaternion(_meshForRotation.transform.rotation.x, 0f, 0f, 1f);
        }

        private void OnGameStateService(GameState state)
        {
            switch (state)
            {
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameFinished()
        {
            _rotating.Stop();
            _figureOfEightRotation.Stop();
            ResetRotationByYZ();
        }

        private void OnCollidedWithGround()
        {
            _figureOfEightRotation.Run(FigureOfEightRotation());

            if (_isRotate == false)
            {
                _rotating.Run(Rotate());
                _isRotate = true;
            }
        }
    }
}