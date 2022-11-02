using System;
using System.Linq;
using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;

namespace Core.Wheel
{
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(AnimationWheel))]
    [RequireComponent(typeof(CollisionHandler))]
    public class Player : MonoBehaviour
    {
        private Movement _movement;
        private AnimationWheel _animation;
        private CollisionHandler _collisionHandler;
        private Wallet _wallet;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _animation = GetComponent<AnimationWheel>();
            _collisionHandler = GetComponent<CollisionHandler>();
        }

        private void OnEnable()
        {
            _collisionHandler.CollidedWithObstacle += AddMoney;
        }

        private void OnDisable()
        {
            _collisionHandler.CollidedWithObstacle -= AddMoney;
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, Wallet wallet, AimDirection aimDirection, Parametr[] parametrs)
        {
            Parametr speedIncrease = parametrs.Where(parameter => parameter.Name == ParameretName.GetName(ParametrType.Speed)).First()
                ?? throw new NullReferenceException($"{typeof(Player)}: Initialize(GameStateService gameStateService, AimDirection aimDirection, Parametr[] parametrs): {nameof(ParametrType.Speed)} is null.");

            _movement.Initialize(gameStateService, coroutineService, aimDirection, speedIncrease);
            _animation.Initialize(gameStateService, coroutineService);
            _collisionHandler.Initialize(gameStateService);
            _wallet = wallet;
        }
        private void AddMoney(int reward)
        {
            _wallet.AddMoney(reward);
        }

    }
}