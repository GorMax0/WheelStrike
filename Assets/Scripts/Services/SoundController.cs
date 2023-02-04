using System;
using UnityEngine;
using Services.GameStates;
using Core.Wheel;
using UnityEngine.UI;
using GameAnalyticsSDK;

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

        private static bool _isMuted;
        private static float _minVolume = 0f;
        private static float _maxVolume = 1f;

        private AudioSource _mainAudioSource;
        private GameStateService _gameStateService;
        private float _initialWheelSpeed;
        private float _cacheVolume;
        private bool _isInitialized;

        public event Action<bool> MutedChanged;

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
            _isMuted = isMuted;
            _mutedSwitcher.isOn = isMuted;
            AudioListener.volume = isMuted == true ? _minVolume : _maxVolume;
        }

        public void SwitchMuted(bool isMuted)
        {
            _isMuted = isMuted;
            AudioListener.volume = _isMuted == true ? _minVolume : _maxVolume;
            MutedChanged?.Invoke(_isMuted);
            GameAnalytics.NewDesignEvent($"guiClick:Sound:{!_isMuted}");
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

        private void DecreaseMainAudioSourceVolume() => _mainAudioSource.volume = _movementWheel.Speed / _initialWheelSpeed * _maxVolume;

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
            if (inBackground == true)
                _cacheVolume = AudioListener.volume;

            AudioListener.pause = inBackground;

            if (inBackground == true)
                AudioListener.volume = _minVolume;
            else
                AudioListener.volume = _cacheVolume;
        }

        public static void ChangeWhenAd(bool _isShowAd)
        {
            AudioListener.pause = _isShowAd;

            if (_isShowAd == true)
                AudioListener.volume = _minVolume;
            else
                AudioListener.volume = _isMuted == true ? _minVolume : _maxVolume;
        }
    }
}