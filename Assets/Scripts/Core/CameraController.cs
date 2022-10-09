using UnityEngine;
using Services.GameStates;
using Cinemachine;
using Zenject;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _gameCamera;
        [SerializeField] private CinemachineVirtualCamera _launchCamera;

        private GameStateService _gameStateService;

        [Inject]
        private void Construct(GameStateService gameStateService)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += OnGameStateChanged;
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

        private void OnGameRunning()
        {
            _launchCamera.gameObject.SetActive(false);
            _gameCamera.gameObject.SetActive(true);
        }
    }
}