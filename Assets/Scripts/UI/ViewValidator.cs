using UnityEngine;
using UnityEngine.Events;
using UI.Views.Finish;

namespace UI
{
    public class ViewValidator : MonoBehaviour
    {
        [SerializeField] private FinishView _viewPortret;
        [SerializeField] private FinishView _viewLandscape;

        private FinishView _currentView;
        private float _screenWidth;
        private float _screenHeight;

        public event UnityAction<FinishView> ViewValidated;

        private void Start()
        {
            DisplayValidView();
            CacheWidthAndHeight();
        }

        private void LateUpdate()
        {
            if (_currentView.gameObject.activeInHierarchy == false)
                return;

            if (_screenWidth == Screen.width && _screenHeight == Screen.height)
                return;

            DisplayValidView();
            CacheWidthAndHeight();

            Debug.Log($"{Screen.width}x{Screen.height}");
        }

        private void DisplayValidView()
        {
            _currentView = Screen.width > Screen.height ? _viewLandscape : _viewPortret;
            ViewValidated?.Invoke(_currentView);
        }

        private void CacheWidthAndHeight()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }
    }
}