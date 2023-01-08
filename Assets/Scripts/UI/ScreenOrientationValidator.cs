using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ScreenOrientationValidator : MonoBehaviour
    {
        private float _screenWidth;
        private float _screenHeight;
        private bool _hasPortrietOrientation;

        public event UnityAction<bool> OrientationValidated;

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
            _hasPortrietOrientation = Screen.width < Screen.height ? true : false;
            OrientationValidated?.Invoke(_hasPortrietOrientation);
        }

        private void CacheScreeSizes()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }
    }
}