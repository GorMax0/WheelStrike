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
        [SerializeField] private CollisionHandler _collisionHandler;

        private GameStateService _gameStateService;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _collisionHandler.CollidedWithGround += ChangeFOV;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _collisionHandler.CollidedWithGround -= ChangeFOV;
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;
            OnEnable();
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

        private void ChangeFOV()
        {
            const float NarrowingOfFOV = 4f;
            const float MinFOV = 84;
            float newFOV = Mathf.Clamp(_gameCamera.m_Lens.FieldOfView - NarrowingOfFOV, MinFOV, _gameCamera.m_Lens.FieldOfView);

            _gameCamera.m_Lens.FieldOfView = newFOV;
        }
    }
}