using System;
using UnityEngine;
using Services.GameStates;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Car : MonoBehaviour
    {
        private const float Speed = 1f;

        private Rigidbody _rigidbody;
        private MeshRenderer[] _meshRenders;
        private GameStateService _gameStateService;
        private bool _isInitialized;

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

        public void Initialize(GameStateService gameStateService, Material colorMaterial)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{typeof(Car)}: Initialize(GameStateService gameStateService, Material colorMaterial): Already initialized.");

            _gameStateService = gameStateService;
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenders = GetComponentsInChildren<MeshRenderer>();

            SetColorMaterial(colorMaterial);
            OnEnable();

            _isInitialized = true;
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

        private void Move()
        {
            _rigidbody.velocity = transform.forward * Speed;
        }

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Running:
                    OnGameRunning();
                    break;
            }
        }

        private void OnGameRunning()
        {
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }
}
