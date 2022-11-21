using System;
using System.Linq;
using UnityEngine;
using Parameters;
using Services.Coroutines;
using Services.GameStates;

namespace Core.Wheel
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Movement))]
    [RequireComponent(typeof(AnimationWheel))]
    [RequireComponent(typeof(InteractionHandler))]
    public class Player : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Movement _movement;
        private AnimationWheel _animation;
        private InteractionHandler _collisionHandler;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _movement = GetComponent<Movement>();
            _animation = GetComponent<AnimationWheel>();
            _collisionHandler = GetComponent<InteractionHandler>();
        }

        public void Initialize(GameStateService gameStateService, CoroutineService coroutineService, AimDirection aimDirection, Parameter[] parameters)
        {
            Parameter speedIncrease = parameters.Where(parameter => parameter.Name == ParameretName.GetName(ParameterType.Speed)).First()
                ?? throw new NullReferenceException($"{typeof(Player)}: Initialize(GameStateService gameStateService, AimDirection aimDirection, Parametr[] parametrs): {nameof(ParameterType.Speed)} is null.");

            _movement.Initialize(gameStateService, coroutineService, aimDirection, speedIncrease);
            _animation.Initialize(gameStateService, coroutineService);
            _collisionHandler.Initialize(gameStateService);
        }
    }
}