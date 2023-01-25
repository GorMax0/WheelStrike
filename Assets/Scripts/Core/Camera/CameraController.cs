using System;
using UnityEngine;
using Cinemachine;
using Core.Wheel;
using Services;
using Services.GameStates;

namespace Core
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _menuCamera;
        [SerializeField] private CinemachineVirtualCamera _launchCamera;
        [SerializeField] private CinemachineVirtualCamera _gameCamera;
        [SerializeField] private CinemachineFreeLook _finishCamera;
        [SerializeField] private CinemachineVirtualCamera _lookWall;
        [SerializeField] private CinemachineVirtualCamera _carLook;
        [SerializeField] private CameraTrigger _cameraTrigger;

        private const float MinimumFieldOfView = 70f;
        private const float InterpolateValueFieldOfView = 0.02f;
        private const float SpeedRotationXAxis = 0.5f;

        private GameStateService _gameStateService;
        private GamePlayService _gamePlayService;
        private InteractionHandler _interactionHandler;
        private bool _isInitialized;
        private bool _isFinished;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _gamePlayService.TriggeredCar += OnTriggeredCar;
            _gamePlayService.TimeChangedToDefault += OnTimeChangedToDefault;
            _interactionHandler.CollidedWithGround += OnCollidedWithGrounds;
            _cameraTrigger.WheelTriggered += OnWheelTriggered;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _gamePlayService.TriggeredCar -= OnTriggeredCar;
            _gamePlayService.TimeChangedToDefault -= OnTimeChangedToDefault;
            _interactionHandler.CollidedWithGround -= OnCollidedWithGrounds;
            _cameraTrigger.WheelTriggered -= OnWheelTriggered;
        }

        private void Update()
        {
            if (_isFinished == false)
                return;

            EnableFinishingRotation();
        }

        private void EnableFinishingRotation()
        {
            _finishCamera.m_Lens.FieldOfView = Mathf.Lerp(_finishCamera.m_Lens.FieldOfView, MinimumFieldOfView, InterpolateValueFieldOfView);
            _finishCamera.m_XAxis.Value += SpeedRotationXAxis;
        }

        public void Initialize(GameStateService gameStateService, GamePlayService gamePlayService, InteractionHandler interactionHandler)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, GamePlayService gamePlayService, InteractionHandler interactionHandler): Already initialized.");

            _gameStateService = gameStateService;
            _gamePlayService = gamePlayService;
            _interactionHandler = interactionHandler;
            _isInitialized = true;
            OnEnable();
        }

        private void ChangeFieldOfViewForGameCamera()
        {
            const float NarrowingOfFieldOfView = 5f;            
            float newFieldOfView = Mathf.Clamp(_gameCamera.m_Lens.FieldOfView - NarrowingOfFieldOfView, MinimumFieldOfView, _gameCamera.m_Lens.FieldOfView);

            _gameCamera.m_Lens.FieldOfView = newFieldOfView;
        }

        private void SwitchCamers(CinemachineVirtualCameraBase cameraToDisable, CinemachineVirtualCameraBase cameraToEnable)
        {
            cameraToDisable.gameObject.SetActive(false);
            cameraToEnable.gameObject.SetActive(true);
        }

        private void SwitchCamers(CinemachineVirtualCameraBase cameraOneToDisable, CinemachineVirtualCameraBase cameraTwoToDisable, CinemachineVirtualCameraBase cameraToEnable)
        {
            cameraOneToDisable.gameObject.SetActive(false);
            cameraTwoToDisable.gameObject.SetActive(false);
            cameraToEnable.gameObject.SetActive(true);
        }

        private void SetFollow(CinemachineVirtualCameraBase camera, Car car) => camera.Follow = car.transform;

        private void SetLookAt(CinemachineVirtualCameraBase camera, Car car) => camera.LookAt = car.transform;

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
                case GameState.TutorialStepZero:
                    OnGameTutorialStepZero();
                    break;
                case GameState.TutorialStepThree:
                    OnGameTutorialStepThree();
                    break;

            }
        }

        private void OnGameWaiting() => SwitchCamers(_menuCamera, _launchCamera);

        private void OnGameRunning() => SwitchCamers(_launchCamera, _gameCamera);

        private void OnGameFinished()
        {
            SwitchCamers(_gameCamera, _lookWall, _finishCamera);

            _finishCamera.m_Lens.FieldOfView = _gameCamera.m_Lens.FieldOfView;
            _isFinished = true;
        }

        private void OnGameTutorialStepZero() => SwitchCamers(_menuCamera, _launchCamera);
        
        private void OnGameTutorialStepThree() => SwitchCamers(_launchCamera, _menuCamera);

        private void OnCollidedWithGrounds() => ChangeFieldOfViewForGameCamera();

        private void OnWheelTriggered() => SwitchCamers(_gameCamera, _lookWall);

        private void OnTriggeredCar(Car car)
        {
            SetFollow(_carLook, car);
            SetLookAt(_carLook, car);
            SwitchCamers(_gameCamera, _carLook);
        }

        private void OnTimeChangedToDefault() => SwitchCamers(_carLook, _gameCamera);
    }
}