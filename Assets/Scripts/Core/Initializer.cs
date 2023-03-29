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
using Particles;
using Lean.Localization;
using Agava.YandexGames;
using Boost;

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
        [SerializeField] private QualityToggle _qualityToggle;
        [SerializeField] private LeaderboardsHandler _leaderboardsHandler;

        private GameStateService _gameStateService;
        private GamePlayService _gamePlayService;
        private YandexAuthorization _yandexAuthorization;
        private DateTimeService _dateTimeService;
        private DailyReward _dailyReward;

        [Header("Core")]
        [SerializeField] private CarBuilder _carBuilder;
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
        [SerializeField] private BoostView _boostView;
        [SerializeField] private AuthorizationView _authorizationView;
        [SerializeField] private DailyView _dailyView;

        [Header("Other")]
        [SerializeField] private ParameterObject[] _parameterObjects;
        [SerializeField] private TrailManager _trailManager;
        [SerializeField] private TutorialManager _tutorial;
        [SerializeField] private Fog _fog;

        private ParameterCreater _parameterCreater;
        private Dictionary<ParameterType, Parameter> _parameters;
        private BoostParameter _boost;
        private AimDirection _aimDirection;
        private Wallet _wallet = new Wallet();
        private DataOperator _dataOperator;

        private void Start()
        {
#if !UNITY_EDITOR
         //   Localization.SetLanguage();
#endif
            _parameterCreater = new ParameterCreater();
            _parameters = _parameterCreater.CreateParameters(_parameterObjects);
            _boost = new BoostParameter();

            InitializeServices();
            InitializeCore();
            InitializeView();
            _trailManager.Initialize(_gameStateService);
            _fog.Initialize(_gameStateService);
            InitializeLoad();
            InitializeTutorial();
            ScreenOrientationValidator.Instance.Initialize();

            if (_tutorial == null)
                _gameStateService.ChangeState(GameState.Initializing);
        }

        private void InitializeServices()
        {
            _gameStateService = new GameStateService();
            _levelService.Initialize(_gameStateService, _wheel.Travelable, _interactionHandler, _parameters[ParameterType.Income], _boost);
            _yandexAuthorization = new YandexAuthorization();
            _gamePlayService = new GamePlayService(_gameStateService, _yandexAuthorization, _coroutineService, _inputHandler,
                _interactionHandler, _wheel.Travelable, _levelService, _wallet);
            _adsRewards.Initialize(_gameStateService, _wallet);
            _soundController.Initialize(_gameStateService);
            _leaderboardsHandler?.Initialize(_gamePlayService);
            _dateTimeService = new DateTimeService();
        //    _dateTimeService.LoadDate("29.03.2023 23:06:24"); //Test
            _dailyReward = new DailyReward(_gameStateService, _dateTimeService, _wallet, _parameters[ParameterType.Income]);
        }

        private void InitializeCore()
        {
            float timeCameraBlend = _cinemachine.m_DefaultBlend.BlendTime;

            _aimDirection = new AimDirection(_gameStateService, _coroutineService, timeCameraBlend);

            _carBuilder.CreateCars(_gameStateService);
            _cameraController.Initialize(_gameStateService, _gamePlayService, _interactionHandler);
            _forceScale.Initialize(_gameStateService, _coroutineService);
            _ropeDisconnection.Initialize(_gameStateService);
            _wheel.Initialize(_gameStateService, _coroutineService, _aimDirection,
                _parameters[ParameterType.Speed], _parameters[ParameterType.Size],_boost);
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
            _boostView.Initialize(_gameStateService, _boost, _parameters);
            _finishViewHandler.Initialize(_gameStateService, _coroutineService, _wheel.Travelable, _levelService);
            _authorizationView.Initialize(_yandexAuthorization);
            _dailyView.Initialize(_dailyReward);
        }

        private void InitializeLoad()
        {
            _dataOperator = new DataOperator(_gamePlayService, _levelService, _soundController, _qualityToggle,
                _wallet, _parameters, _boost, _yandexAuthorization, _dateTimeService);
            _dataOperator.Load();
        }

        private void InitializeTutorial()
        {
            TutorialState tutorialState = TutorialState.Start;

            if (PlayerPrefs.HasKey(TutorialData))
                tutorialState = (TutorialState)PlayerPrefs.GetInt(TutorialData);

            _tutorial?.Initialize(_gameStateService, tutorialState);
        }
    }
}