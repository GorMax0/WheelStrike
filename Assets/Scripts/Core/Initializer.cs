using System.Collections.Generic;
using UnityEngine;
using Core.Wheel;
using Parameters;
using Services;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;
using UI.Manual;
using UI.Views;
using UI.Views.Money;
using UI.Views.Finish;
using Data;
using Trail;
using AdsReward;

namespace Core
{
    public class Initializer : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Cinemachine.CinemachineBrain _cinemachine;

        [Header("Services")]
        [SerializeField] private LevelService _levelService;
        [SerializeField] private AdsRewards _adsRewards;

        private GameStateService _gameStateService;
        private GamePlayService _gamePlayService;

        [Header("Core")]
        [SerializeField] private CarBuilder _carFactory;
        [SerializeField] private Wall _wall;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ForceScale _forceScale;
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private Rope _rope;
        [SerializeField] private Player _wheel;
        [SerializeField] private InteractionHandler _interactionHandler;

        [Header("Manual")]
        [SerializeField] private ControlManual _controlManual;
        [SerializeField] private AimManual _aimManual;

        [Header("View")]
        [SerializeField] private FinishViewHandler _finishViewHandler;
        [SerializeField] private PrerunView _prerunView;
        [SerializeField] private AimDirectionView _aimDirectionLine;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private MoneyViewPresenter _moneyPresenter;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private ParametersShop _parametersShop;

        [Header("Other")]
        [SerializeField] private ParameterObject[] _parameterObjects;
        [SerializeField] private TrailManager _trailManager;

        private ParameterCreater _parameterCreater;
        private Dictionary<ParameterType, Parameter> _parameters;
        private AimDirection _aimDirection;
        private Wallet _wallet = new Wallet();
        private DataOperator _dataOperator;

        private void Start()
        {
            _parameterCreater = new ParameterCreater();
            _parameters = _parameterCreater.CreateParameters(_parameterObjects);

            InitializeServices();
            InitializeCore();
            InitializeManual();
            InitializeView();
            _trailManager.Initialize(_gameStateService);
            InitializeLoad();
            _gameStateService.ChangeState(GameState.Initializing);
        }

        private void InitializeServices()
        {
            _levelService.Initialize(_wheel.Travelable, _parameters[ParameterType.Income]);
            _gameStateService = new GameStateService();
            _gamePlayService = new GamePlayService(_gameStateService, _coroutineService, _inputHandler, _interactionHandler,_wheel.Travelable, _levelService, _wallet);
            _adsRewards.Initialize(_wallet);
        }

        private void InitializeCore()
        {
            float timeCameraBlend = _cinemachine.m_DefaultBlend.BlendTime;

            _aimDirection = new AimDirection(_gameStateService, _coroutineService, timeCameraBlend);

            _wall.Create();
            _carFactory.CreateCars(_gameStateService);
            _cameraController.Initialize(_gameStateService, _gamePlayService, _interactionHandler);
            _forceScale.Initialize(_gameStateService, _coroutineService);
            _rope.Initialize(_gameStateService);
            _wheel.Initialize(_gameStateService, _coroutineService, _aimDirection, _parameters[ParameterType.Speed], _parameters[ParameterType.Size]);
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
            _menuView.Initialize(_gameStateService);
            _walletView.Initialize(_wallet);
            _moneyPresenter.Initialize(_interactionHandler);
            _topPanel.Initialize(_gameStateService);
            _parametersShop.Initialize(_parameters, _wallet);
            _finishViewHandler.Initialize(_gameStateService, _coroutineService, _wheel.Travelable, _levelService);
        }

        private void InitializeLoad()
        {
            _dataOperator = new DataOperator(_levelService, _wallet, _parameters);
            _dataOperator.Load();
            _gamePlayService.SetDataOperator(_dataOperator);
        }
    }
}