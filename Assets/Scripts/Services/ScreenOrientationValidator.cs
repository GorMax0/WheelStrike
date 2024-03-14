using System;
using UnityEngine;

namespace Services
{
    public class ScreenOrientationValidator : MonoBehaviour
    {
        public static ScreenOrientationValidator Instance;

        private float _screenWidth;
        private float _screenHeight;
        private bool _hasPortraitOrientation;
        private bool _isInitialize;

        public event Action<bool> OrientationValidated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void LateUpdate()
        {
            if (_screenWidth == Screen.width && _screenHeight == Screen.height)
                return;

            OrientationValidation();
            CacheScreeSizes();
        }

        public void Initialize()
        {
            if (_isInitialize)
                throw new InvalidOperationException($"{GetType()}: Initialize(): Already initialized.");

            OrientationValidation();
            CacheScreeSizes();
            _isInitialize = true;
        }

        private void OrientationValidation()
        {
            _hasPortraitOrientation = Screen.width < Screen.height;
            OrientationValidated?.Invoke(_hasPortraitOrientation);
        }

        private void CacheScreeSizes()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }
    }
}