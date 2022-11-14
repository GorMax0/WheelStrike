using UnityEngine;
using Core.Wheel;
using Services.GameStates;
using Cinemachine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _menuCamera;
        [SerializeField] private CinemachineVirtualCamera _launchCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;
        [SerializeField] private CinemachineFreeLook _finishedCamera;
        [SerializeField] private CollisionHandler _collisionHandler;

        private GameStateService _gameStateService;
        private bool _isFinished;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _collisionHandler.CollidedWithGround += OnCollidedWithGrounds;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _collisionHandler.CollidedWithGround -= OnCollidedWithGrounds;
        }

        private void Update()
        {
            if (_isFinished == false)
                return;

            _finishedCamera.m_Lens.FieldOfView = Mathf.Lerp(_finishedCamera.m_Lens.FieldOfView, 60f, 5f);
            _finishedCamera.m_XAxis.Value += 0.5f;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            OnEnable();
        }

        private void ChangeFOV()
        {
            const float NarrowingOfFOV = 4f;
            const float MinFOV = 84;
            float newFOV = Mathf.Clamp(_gameCamera.m_Lens.FieldOfView - NarrowingOfFOV, MinFOV, _gameCamera.m_Lens.FieldOfView);

            _gameCamera.m_Lens.FieldOfView = newFOV;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Waiting:
                    OnGameWaiting();
                    break;
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameWaiting()
        {
            _menuCamera.gameObject.SetActive(false);
            _launchCamera.gameObject.SetActive(true);
        }

        private void OnGameRunning()
        {
            _launchCamera.gameObject.SetActive(false);
            _gameCamera.gameObject.SetActive(true);
        }

        private void OnGameFinished()
        {
            _gameCamera.gameObject.SetActive(false);
            _finishedCamera.gameObject.SetActive(true);

            _finishedCamera.m_Lens.FieldOfView = _gameCamera.m_Lens.FieldOfView;
            _isFinished = true;
        }

        private void OnCollidedWithGrounds()
        {
            ChangeFOV();
        }
    }
}