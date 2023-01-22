using System;
using UnityEngine;
using Services.GameStates;
using Core.Wheel;
using UnityEngine.UI;

namespace Services
{
    [RequireComponent(typeof(SoundController))]
    public class SoundController : MonoBehaviour
    {
        [SerializeField] private AudioClip _music;
        [SerializeField] private AudioClip _wind;
        [SerializeField] private AudioClip _swingWheel;
        [SerializeField] private AudioClip _runWheel;
        [SerializeField] private AudioClip _finish;
        [SerializeField] private Movement _movementWheel;
        [SerializeField] private AudioSource _craneAudioSource;
        [SerializeField] private Toggle _mutedSwitcher;

        private AudioSource _mainAudioSource;
        private GameStateService _gameStateService;
        private float _minVolume = 0f;
        private float _maxVolume = 1f;
        private float _initialWheelSpeed;
        private bool _isMuted;
        private bool _isInitialized;

        public event Action<bool> MutedChanged;

        public bool IsMute => _isMuted;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            Agava.WebUtility.WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            Agava.WebUtility.WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }

        private void Update()
        {
            if (_movementWheel == null)
                return;

            if (HasInitialWheelSpeedNotZero() == false)
                return;

            DecreaseMainAudioSourceVolume();
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(GameStateService gameStateService): Already initialized.");

            _mainAudioSource = GetComponent<AudioSource>();
            _gameStateService = gameStateService;

            _isInitialized = true;
            OnEnable();
        }

        public void LoadMutedState(bool isMuted)
        {
            _mutedSwitcher.isOn = isMuted;
            AudioListener.volume = isMuted == true ? _minVolume : _maxVolume;
        }

        public void SwitchMuted(bool isMuted)
        {
            _isMuted = isMuted;
            AudioListener.volume = _isMuted == true ? _minVolume : _maxVolume;
            MutedChanged?.Invoke(_isMuted);            
        }

        private bool HasInitialWheelSpeedNotZero()
        {
            if (_initialWheelSpeed == 0)
            {
                _initialWheelSpeed = _movementWheel.Speed;
                return false;
            }

            return true;
        }

        private void DecreaseMainAudioSourceVolume() => _mainAudioSource.volume = _movementWheel.Speed / _initialWheelSpeed;

        private void OnGameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Initializing:
                    OnGameInitializing();
                    break;
                case GameState.Waiting:
                    OnGameWaiting();
                    break;
                case GameState.Running:
                    OnGameRunning();
                    break;
                case GameState.Finished:
                    OnGameFinished();
                    break;
            }
        }

        private void OnGameInitializing()
        {
            _mainAudioSource.clip = _music;
            _mainAudioSource.Play();
        }

        private void OnGameWaiting()
        {
            _mainAudioSource.Stop();
            _mainAudioSource.clip = _swingWheel;
            _mainAudioSource.Play();
        }

        private void OnGameRunning()
        {
            _craneAudioSource.clip = _runWheel;
            _craneAudioSource.Play();
            _mainAudioSource.Stop();
            _mainAudioSource.clip = _wind;
            _mainAudioSource.Play();
        }

        private void OnGameFinished()
        {
            _mainAudioSource.Stop();
            _craneAudioSource.clip = _finish;
            _craneAudioSource.Play();
        }

        private void OnInBackgroundChange(bool inBackground)
        {            
            AudioListener.pause = inBackground;
            AudioListener.volume = inBackground ? 0f : 1f;
        }
    }
}