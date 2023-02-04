using Core.Wheel;
using Services;
using Services.GameStates;
using UnityEngine;

namespace Particles
{
    public class Fog : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fog;
        [SerializeField] private Movement _wheel;
        [SerializeField] private QualityToggle _qualityToggle;

        private GameStateService _gameStateService;
        private float _distanceToWheel;
        private bool _isRun;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _qualityToggle.QualityChanged += OnQualityChanged;
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _qualityToggle.QualityChanged -= OnQualityChanged;
        }

        private void FixedUpdate()
        {
            if (_isRun == false)
                return;

            transform.position = new Vector3(transform.position.x, transform.position.y, _wheel.transform.position.z + _distanceToWheel);
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_isInitialized == true)
                return;

            _gameStateService = gameStateService;
            _distanceToWheel = transform.position.z - _wheel.transform.position.z;
            _isInitialized = true;
            OnEnable();
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.Running)
                _isRun = true;
        }

        private void OnQualityChanged(bool isNormalQuality)
        {
            if (isNormalQuality == false)
                _fog.Play();
            else
                _fog.Stop();
        }
    }
}