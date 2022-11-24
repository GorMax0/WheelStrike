using UnityEngine;
using Core.Wheel;
using Parameters;
using Services;
using Services.Coroutines;
using Services.GameStates;
using UI;
using UI.Manual;
using UI.Views;

namespace Core
{
    public class Initializer : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Cinemachine.CinemachineBrain _cinemachine;

        [Header("Core")]
        [SerializeField] private CarFactory _carFactory;
        [SerializeField] private Wall _wall;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ForceScale _forceScale;
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private ParameterCreater[] _parameterCreaters;
        [SerializeField] private Rope _rope;
        [SerializeField] private Player _wheel;
        [SerializeField] private InteractionHandler _interactionHandler;
        
        [Header("Manual")]
        [SerializeField] private ControlManual _controlManual;
        [SerializeField] private AimManual _aimManual;

        [Header("View")]
        [SerializeField] private ViewValidator _finishViewValidator;
        [SerializeField] private PrerunView _prerunView;
        [SerializeField] private AimDirectionView _aimDirectionLine;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private UpgradePanel _upgradePanel;

        private GameStateService _gameStateService;
        private GamePlayService _gamePlayService;
        private LevelService _levelService;
        private AimDirection _aimDirection;
        private Parameter[] _parameters;
        private Wallet _wallet;
        private Finish _finish;

        private void Start()
        {
            CreateParameters();
            InitializeCore();
            InitializeManual();
            InitializeView();
            _gameStateService.ChangeState(GameState.Pause);
        }

        private void InitializeCore()
        {
            float timeCameraBlend = _cinemachine.m_DefaultBlend.BlendTime;

            _wallet = new Wallet(_parameters);
            _gameStateService = new GameStateService();
            _gamePlayService = new GamePlayService(_gameStateService, _inputHandler,_interactionHandler, _wallet);
            _levelService = new LevelService();
            _aimDirection = new AimDirection(_gameStateService, _coroutineService, timeCameraBlend);
            _finish = new Finish(_gameStateService, _finishViewValidator);

            _wall.Create();
            _carFactory.CreateCars(_gameStateService);
            _cameraController.Initialize(_gameStateService);
            _forceScale.Initialize(_gameStateService, _coroutineService);
            _rope.Initialize(_gameStateService);
            _wheel.Initialize(_gameStateService, _coroutineService, _aimDirection, _parameters);
        }

        private void InitializeManual()
        {
            _controlManual.Initialize(_gameStateService);
            _aimManual.Initialize(_coroutineService);
        }

        private void InitializeView()
        {
            _prerunView.Initialize(_gameStateService);
            _aimDirectionLine.Initialize(_aimDirection);
            _pauseView.Initialize(_gameStateService);
            _moneyView.Initialize(_wallet);
            _topPanel.Initialize(_gameStateService, _levelService);
            _upgradePanel.Initialize(_parameters);
        }

        private void CreateParameters()
        {
            _parameters = new Parameter[_parameterCreaters.Length];

            for (int i = 0; i < _parameterCreaters.Length; i++)
            {
                _parameters[i] = new Parameter(_parameterCreaters[i]);
            }
        }
    }
}