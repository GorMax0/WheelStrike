using UnityEngine;
using Services.Coroutines;
using Services.GameStates;
using System.Collections;

namespace Core.Wheel
{
    [RequireComponent(typeof(ForceScale))]
    [RequireComponent(typeof(CollisionHandler))]
    public class AnimationWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _deviationWhenSwinging;
        [SerializeField] private MeshRenderer _meshForRotation;
        [SerializeField] private float _rotationSpeed;

        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private CollisionHandler _collisionHandler;
        private CoroutineRunning _rotating;
        private CoroutineRunning _figureOfEightRotation;

        private void Awake()
        {
            _forceScale = GetComponent<ForceScale>();
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        private void OnEnable()
        {
            if (_gameStateService == null)
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
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            _rotating = new CoroutineRunning(coroutineService);
            _figureOfEightRotation = new CoroutineRunning(coroutineService);

            OnEnable();
        }

        private void Swing(float currentForceValue)
        {
            float swingValue = _deviationWhenSwinging.Evaluate(currentForceValue);

            transform.position = new Vector3(transform.position.x, transform.position.y, swingValue);
        }

        private IEnumerator Rotation()
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
            _rotating.Run(Rotation());
        }

        private void OnGameFailed()
        {
            _rotating.Stop();
            _figureOfEightRotation.Stop();
            ResetRotationByYZ();
        }

        private void OnCollidedWithGround()
        {
            _figureOfEightRotation.Run(FigureOfEightRotation());
        }    
    }
}