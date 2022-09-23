using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameStateService _gameStateService;
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private LevelService _levelService;

    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle();
        Container.Bind<LevelService>().AsSingle();
        Container.Bind<InputHandler>().AsSingle();
    }
}
