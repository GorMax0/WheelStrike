using System;
using UnityEngine;
using DG.Tweening;
using Services.GameStates;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Explosion))]
    [RequireComponent(typeof(AudioSource))]
    public class Car : MonoBehaviour
    {
        [SerializeField] private int _minRandomReward;
        [SerializeField] private int _maxRandomReward;

        private const int FullPercent = 100;
        private const float Speed = 3.5f;

        private Rigidbody _rigidbody;
        private MeshRenderer[] _meshRenders;
        private Explosion _explosion;
        private CarWheel[] _carWheels;
        private AudioSource _audioSource;
        private GameStateService _gameStateService;
        private bool _isInitialized = false;

        public event Action<bool> IsMovable;

        [field: SerializeField] public int Reward { get; private set; }

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
        }

        public void Initialize(GameStateService gameStateService, Material colorMaterial)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService, Material colorMaterial): Already initialized.");

            _gameStateService = gameStateService;
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenders = GetComponentsInChildren<MeshRenderer>();
            _explosion = GetComponent<Explosion>();
            _audioSource = GetComponent<AudioSource>();
            _carWheels = GetComponentsInChildren<CarWheel>();

            InitializeWheels();

            SetColorMaterial(colorMaterial);
            RandomizeRewardIncrease();

            _isInitialized = true;
            OnEnable();
        }

        public void Explode()
        {
            _explosion.Explode();

            foreach (MeshRenderer meshRenderer in _meshRenders)
            {
                meshRenderer.material.DOColor(Color.black, 0.3f);
                meshRenderer.material.DisableKeyword("_EMISSION");
            }

            _audioSource.Play();
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

        private void RandomizeRewardIncrease() => Reward += (Reward * UnityEngine.Random.Range(_minRandomReward, _maxRandomReward)) / FullPercent;

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
