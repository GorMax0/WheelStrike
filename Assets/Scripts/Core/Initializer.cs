using System.Collections.Generic;
using UnityEngine;
using Core.Wheel;
using Parameters;
using Services;
using Services.Coroutines;
using Services.GameStates;
using Services.Level;
using UI.Manual;
using UI.Manual.Tutorial;
using UI.Views;
using UI.Views.Money;
using UI.Views.Finish;
using Data;
using Trail;
using AdsReward;
using Leaderboards;
using Authorization;

namespace Core
{
    public class Initializer : MonoBehaviour
    {
        private const string TutorialData = "Tutorial";

        [Header("Camera")]
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Cinemachine.CinemachineBrain _cinemachine;

        [Header("Services")]
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private LevelService _levelService;
        [SerializeField] private AdsRewards _adsRewards;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private LeaderboardsHandler _leaderboardsHandler;

        private GameStateService _gameStateService;
        private GamePlayService _gamePlayService;
        private YandexAuthorization _yandexAuthorization;

        [Header("Core")]
        [SerializeField] private CarBuilder _carBuilder;
        [SerializeField] private Wall _wall;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ForceScale _forceScale;
        [SerializeField] private RopeDisconnection _ropeDisconnection;
        [SerializeField] private Player _wheel;
        [SerializeField] private InteractionHandler _interactionHandler;

        [Header("View")]
        [SerializeField] private ControlManual _controlManual;
        [SerializeField] private FinishViewHandler _finishViewHandler;
        [SerializeField] private PrerunView _prerunView;
        [SerializeField] private AimDirectionView _aimDirectionLine;
        [SerializeField] private MenuView _menuView;
        [SerializeField] private WalletView _walletView;
        [SerializeField] private MoneyViewPresenter _moneyPresenter;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private ParametersShop _parametersShop;
        [SerializeField] private AuthorizationView _authorizationView;

        [Header("Other")]
        [SerializeField] private ParameterObject[] _parameterObjects;
        [SerializeField] private TrailManager _trailManager;
        [SerializeField] private TutorialManager _tutorial;

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
            InitializeView();
            _trailManager.Initialize(_gameStateService);
            InitializeLoad();
            InitializeTutorial();
            ScreenOrientationValidator.Instance.Initialize();

            if (_tutorial == null)
                _gameStateService.ChangeState(GameState.Initializing);
        }

        private void InitializeServices()
        {
            _levelService.Initialize(_wheel.Travelable, _parameters[ParameterType.Income]);
            _gameStateService = new GameStateService();
            _yandexAuthorization = new YandexAuthorization();
            _gamePlayService = new GamePlayService(_gameStateService, _yandexAuthorization, _coroutineService, _inputHandler,
                _interactionHandler, _wheel.Travelable, _levelService, _wallet);
            _adsRewards.Initialize(_wallet);
            _soundController.Initialize(_gameStateService);
            _leaderboardsHandler?.Initialize(_gamePlayService);
        }

        private void InitializeCore()
        {
            float timeCameraBlend = _cinemachine.m_DefaultBlend.BlendTime;

            _aimDirection = new AimDirection(_gameStateService, _coroutineService, timeCameraBlend);

            _wall.Create();
            _carBuilder.CreateCars(_gameStateService);
            _cameraController.Initialize(_gameStateService, _gamePlayService, _interactionHandler);
            _forceScale.Initialize(_gameStateService, _coroutineService);
            _ropeDisconnection.Initialize(_gameStateService);
            _wheel.Initialize(_gameStateService, _coroutineService, _aimDirection,
                _parameters[ParameterType.Speed], _parameters[ParameterType.Size]);
        }

        private void InitializeView()
        {
            _controlManual.Initialize(_gameStateService, _coroutineService);
            _prerunView.Initialize(_gameStateService);
            _aimDirectionLine.Initialize(_aimDirection);
            _menuView.Initialize(_gameStateService);
            _walletView.Initialize(_wallet);
            _moneyPresenter.Initialize(_interactionHandler);
            _topPanel.Initialize(_gameStateService, _coroutineService);
            _parametersShop.Initialize(_parameters, _wallet);
            _finishViewHandler.Initialize(_gameStateService, _coroutineService, _wheel.Travelable, _levelService);
            _authorizationView.Initialize(_yandexAuthorization);
        }

        private void InitializeLoad()
        {
            _dataOperator = new DataOperator(_gamePlayService, _levelService, _soundController,
                _wallet, _parameters, _yandexAuthorization);
            _dataOperator.Load();
        }

        private void InitializeTutorial()
        {
            TutorialState tutorialState = TutorialState.Start;

            if (PlayerPrefs.HasKey(TutorialData))
                tutorialState = (TutorialState)PlayerPrefs.GetInt(TutorialData);

            if (tutorialState == TutorialState.FullCompleted)
                return;

            _tutorial?.Initialize(_gameStateService, tutorialState);
        }
    }
}