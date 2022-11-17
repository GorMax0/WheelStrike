using UnityEngine;
using Core.Wheel;
using Services.GameStates;
using Cinemachine;
using System;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _menuCamera;
        [SerializeField] private CinemachineVirtualCamera _launchCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;
        [SerializeField] private CinemachineFreeLook _finishedCamera;
        [SerializeField] private CinemachineVirtualCamera _lookWall;
        [SerializeField] private CameraTrigger _cameraTrigger;
        [SerializeField] private InteractionHandler _collisionHandler;

        private const float MinimumFOV = 60f;
        private const float SpeedRotationXAxis = 0.5f;
        private const float InterpolateValueFOV = 0.02f;

        private GameStateService _gameStateService;
        private bool _isFinished;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _cameraTrigger.WheelTriggered += OnWheelTriggered;
            _collisionHandler.CollidedWithGround += OnCollidedWithGrounds;
        }


        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _cameraTrigger.WheelTriggered -= OnWheelTriggered;
            _collisionHandler.CollidedWithGround -= OnCollidedWithGrounds;
        }

        private void Update()
        {
            if (_isFinished == false)
                return;

            _finishedCamera.m_Lens.FieldOfView = Mathf.Lerp(_finishedCamera.m_Lens.FieldOfView, MinimumFOV, InterpolateValueFOV);
            _finishedCamera.m_XAxis.Value += SpeedRotationXAxis;
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
            const float NarrowingOfFOV = 5f;
            const float MinFOV = 80;
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
            _lookWall.gameObject.SetActive(false);
            _finishedCamera.gameObject.SetActive(true);

            _finishedCamera.m_Lens.FieldOfView = _gameCamera.m_Lens.FieldOfView;
            _isFinished = true;
        }

        private void OnCollidedWithGrounds()
        {
            ChangeFOV();
        }

        private void OnWheelTriggered()
        {
            _gameCamera.gameObject.SetActive(false);
            _lookWall.gameObject.SetActive(true);
        }
    }
}