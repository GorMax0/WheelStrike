using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private ParametrCreater[] _parametrCreaters;
    [SerializeField] private ForceScale _forceScale;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LevelService>().AsSingle();
        Container.BindInterfacesTo<GamePlayService>().AsSingle();
        Container.BindInterfacesAndSelfTo<AimDirection>().AsSingle();
        Container.BindInstance(_inputHandler).AsSingle();
        Container.BindInstance(_forceScale).AsSingle();
        Container.Bind<Wallet>().AsSingle();
        BindParameters();
    }

    private void BindParameters()
    {
        foreach (ParametrCreater creater in _parametrCreaters)
        {
            Container.BindInstance(new Parametr(creater));
        }
    }
}
