using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ScreenOrientationValidator : MonoBehaviour
    {
        public static ScreenOrientationValidator Instance;

        private float _screenWidth;
        private float _screenHeight;
        private bool _hasPortraitOrientation;

        public event UnityAction<bool> OrientationValidated;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            OrientationValidation();
            CacheScreeSizes();
        }

        private void LateUpdate()
        {
            if (_screenWidth == Screen.width && _screenHeight == Screen.height)
                return;

            OrientationValidation();
            CacheScreeSizes();
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