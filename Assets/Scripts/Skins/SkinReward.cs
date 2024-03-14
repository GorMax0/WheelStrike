using System;
using Services.GameStates;
using UnityEngine;
using UnityEngine.UI;

namespace Skins
{
    public class SkinReward : MonoBehaviour
    {
        [SerializeField] private Button _takeButton;

        private GameStateService _stateService;

        public event Action<int> OnRewarded;

        public bool IsRewarded { get; private set; }

        private void OnEnable()
        {
            _takeButton.onClick.AddListener(Take);
        }

        private void OnDisable()
        {
            _takeButton.onClick.RemoveListener(Take);
        }

        public void Initialize(GameStateService stateService)
        {
            _stateService = stateService;
            _stateService.GameStateChanged += CheckReward;
        }

        public void Load(bool isRewarded)
        {
            IsRewarded = isRewarded;
        }

        private void CheckReward(GameState state)
        {
            if (state != GameState.Initializing)
                return;

            gameObject.SetActive(!IsRewarded);
        }

        private void Take()
        {
            IsRewarded = true;
            OnRewarded?.Invoke(1);
            _stateService.ChangeState(GameState.Save);
            gameObject.SetActive(false);
        }
    }
}