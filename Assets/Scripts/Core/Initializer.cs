using UnityEngine;
using Parameters;
using Services;
using Services.Coroutines;
using Services.GameStates;

namespace Core
{
    public class Initializer : MonoBehaviour
    {
        [SerializeField] private CoroutineService _coroutineService;
        [SerializeField] private InputHandler _inputHandler;
        [SerializeField] private ParametrCreater[] _parametrCreaters;
        [SerializeField] private ForceScale _forceScale;
        [SerializeField] private Cinemachine.CinemachineBrain _cinemachine;

        private GameStateService _gameStateService = new GameStateService();
        private GamePlayService _gamePlayService;
        private LevelService _levelService;

        //Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle(); +++
        //Container.BindInterfacesAndSelfTo<LevelService>().AsSingle(); +++
        //Container.BindInterfacesTo<GamePlayService>().AsSingle(); +++
        //Container.Bind<CoroutineService>().FromNewComponentOnNewGameObject().AsSingle(); +++
        //Container.BindInterfacesAndSelfTo<AimDirection>().AsSingle();

        //Container.Bind<Wallet>().AsSingle();

        private void Start()
        {
            _gamePlayService = new GamePlayService(_gameStateService, _inputHandler);
            _levelService = new LevelService();
        }


        private void CreateParameters()
        {
            foreach (ParametrCreater creater in _parametrCreaters)
            {
                //Container.BindInstance(new Parametr(creater));
            }
        }
    }
}