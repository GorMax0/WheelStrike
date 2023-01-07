using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Services.GameStates;
using System;

namespace UI.Views
{
    public class TopPanel : MonoBehaviour
    {
        [SerializeField] private Button _settingButton;
        [SerializeField] private Wrap _restartButton;
        [SerializeField] private Wrap _moneyPanel;
        [SerializeField] private Wrap _distancePanel;
        [SerializeField] private Image _curtain;

        private GameStateService _gameStateService;
        private float _transparency = 0f;
        private float _notTransparency = 1f;
        private float _delayFade = 0.3f;

        private void OnEnable()
        {
            if (_gameStateService == null)
                return;

            _gameStateService.GameStateChanged += OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _gameStateService.GameStateChanged -= OnGameStateChanged;
            _restartButton.GetComponent<Button>().onClick.RemoveListener(Restart);
        }

        public void Initialize(GameStateService gameStateService)
        {
            if (_gameStateService != null)
                return;

            _gameStateService = gameStateService;

            OnEnable();
        }

        public void Restart()
        {
            EnableCurtain();
            _curtain.DOFade(_notTransparency, _delayFade);
            Invoke(nameof(SetGameStateRestart), _delayFade);
        }

        private void EnableCurtain() => _curtain.gameObject.SetActive(true);
        private void DisableCurtain() => _curtain.gameObject.SetActive(false);


        private void SetGameStateRestart() => _gameStateService.ChangeState(GameState.Restart);

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
            EnableCurtain();

            DOTween.Sequence().
               Append(_curtain.DOFade(_transparency, _delayFade))
               .AppendInterval(_delayFade)
               .AppendCallback(DisableCurtain);
        }

        private void OnGameWaiting()
        {
            _moneyPanel.ApplyOffsetTransform();
        }
        private void OnGameRunning()
        {
            _restartButton.ApplyOffsetTransform();
            _distancePanel.ApplyOffsetTransform();
        }

        private void OnGameFinished()
        {
            _distancePanel.CancelOffsetTransform();
            _restartButton.CancelOffsetTransform();
        }
    }
}