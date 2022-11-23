using UnityEngine;
using UI.Views;
using Services.GameStates;
using System;

namespace UI
{
    public class ViewValidation : MonoBehaviour
    {
        [SerializeField] private FinishView _viewPortret;
        [SerializeField] private FinishView _viewLandscape;

        private GameStateService _gameStateService;
        private float _screenWidth;
        private float _screenHeight;
        private bool _viewEnabled;

        private void OnEnable()
        {
            _gameStateService.GameStateChanged += OnGameStateChanged;
            _viewPortret.Enabled += Enable;
            _viewLandscape.Enabled += Enable;
        }


        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _viewPortret.Enabled -= Enable;
            _viewLandscape.Enabled -= Enable;
        }

        private void Start()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }

        private void LateUpdate()
        {
            if (_viewEnabled == false)
                return;

            if (_screenWidth == Screen.width && _screenHeight == Screen.height)
                return;

            DisplayValidView();
            CacheWidthAndHeight();

            Debug.Log($"{Screen.width}x{Screen.height}");
        }

        private void DisplayValidView()
        {
            if (Screen.width > Screen.height)
            {
                _viewPortret.gameObject.SetActive(false);
                _viewLandscape.gameObject.SetActive(true);
            }
            else
            {
                _viewLandscape.gameObject.SetActive(false);
                _viewPortret.gameObject.SetActive(true);
            }
        }

        private void CacheWidthAndHeight()
        {
            _screenWidth = Screen.width;
            _screenHeight = Screen.height;
        }

        private void Enable(bool isEnabled)
        {
            _viewEnabled = isEnabled;
        }

        private void OnGameStateChanged(GameState state)
        {
            throw new NotImplementedException();
        }
    }
}