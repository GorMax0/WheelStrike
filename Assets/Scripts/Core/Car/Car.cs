using System;
using UnityEngine;
using Services.GameStates;
using DG.Tweening;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        private const float Speed = 3.5f;

        private Rigidbody _rigidbody;
        private MeshRenderer[] _meshRenders;
        private Material _damageMaterial;
        private Explosion _explosion;
        private CarWheel[] _carWheels;
        private GameStateService _gameStateService;
        private bool _isInitialized;

        public event Action<bool> IsMovable;

        [field: SerializeField] public int Reward { get; private set; }

        public bool IsPurchased { get; private set; }

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService, Material colorMaterial, Material damageMaterial)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{typeof(Car)}: Initialize(GameStateService gameStateService, Material colorMaterial): Already initialized.");

            _gameStateService = gameStateService;
            _damageMaterial = damageMaterial;
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenders = GetComponentsInChildren<MeshRenderer>();
            _explosion = GetComponent<Explosion>();
            _carWheels = GetComponentsInChildren<CarWheel>();

            InitializeWheels();

            SetColorMaterial(colorMaterial);
            RandomizeRewardIncrease();
            OnEnable();

            _isInitialized = true;
        }

        public void Explode()
        {
            _explosion.Explode();

            foreach (MeshRenderer meshRender in _meshRenders)
            {
                meshRender.material.Lerp(meshRender.material,_damageMaterial,2f);
            }
        }

        public void StopMove()
        {
            _rigidbody.velocity = Vector3.zero;
            IsMovable?.Invoke(false);
        }

        private void InitializeWheels()
        {
            foreach (CarWheel wheel in _carWheels)
            {
                wheel.Initialize(this);
            }
        }

        private void SetColorMaterial(Material colorMaterial)
        {
            if (_meshRenders.Length <= 0)
                throw new InvalidOperationException($"{gameObject.name}: SetColorMaterial(Material colorMaterial): {nameof(_meshRenders)} does not contain values.");

            foreach (MeshRenderer meshRender in _meshRenders)
            {
                meshRender.material = colorMaterial;
            }
        }

        private void RandomizeRewardIncrease()
        {
            Reward += (int)(Reward * UnityEngine.Random.value);
        }

        private void Move()
        {
            _rigidbody.velocity = transform.forward * Speed;
            IsMovable?.Invoke(true);
        }



        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameRunning()
        {
            Move();
        }

        private void OnGameFinished()
        {
            StopMove();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Obstacle obstacle))
                StopMove();
        }
    }
}
