using UnityEngine;
using Zenject;
using Services.Coroutines;
using Services.GameStates;
using System.Collections;

namespace Core
{
    [RequireComponent(typeof(ForceScale))]
    public class AnimationWheel : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _deviationWhenSwinging;
        [SerializeField] private MeshRenderer _meshForRotation;
        [SerializeField] private float _turnSpeed;

        private GameStateService _gameStateService;
        private ForceScale _forceScale;
        private CoroutineRunning _rotating;

        private void Awake()
        {
            _forceScale = GetComponent<ForceScale>();
        }

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateService;
            _forceScale.MultiplierChanged += Swing;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateService;
            _forceScale.MultiplierChanged -= Swing;
        }

        [Inject]
        private void Construct(GameStateService gameStateService, CoroutineService coroutineService)
        {
            _gameStateService = gameStateService;
            _rotating = new CoroutineRunning(coroutineService);
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
                _meshForRotation.transform.Rotate(-Vector3.left * Time.deltaTime * _turnSpeed);

                yield return null;
            }
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
            _rotating.Run(Rotation());
        }
    }
}