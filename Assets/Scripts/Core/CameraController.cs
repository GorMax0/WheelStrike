using UnityEngine;
using Services.GameStates;
using Cinemachine;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _menuCamera;
        [SerializeField] private CinemachineVirtualCamera _launchCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;

        private GameStateService _gameStateService;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
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
    }
}