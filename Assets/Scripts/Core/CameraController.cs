using UnityEngine;
using Services.GameStates;
using Cinemachine;
using Zenject;

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
            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        [Inject]
        private void Construct(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
        }

        private void OnGameStateChanged(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Initializing:
                    break;
                case GameState.Pause:
                    break;
                case GameState.Waiting:
                    OnGameWaiting();
                    break;
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Failed:
                    break;
                case GameState.Restart:
                    break;
                case GameState.Winning:
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