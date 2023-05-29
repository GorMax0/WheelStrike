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
        private bool _isRewarded;
        
        public event Action<int> OnRewarded;

        public bool IsRewarded => _isRewarded;
        
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

        private void CheckReward(GameState state)
        {
            if (state != GameState.Initializing)
                return;
            
            gameObject.SetActive(!_isRewarded);
        }

        public void Load(bool isRewarded)
        {
            _isRewarded = isRewarded;
        }

        private void Take()
        {
            _isRewarded = true;
            OnRewarded?.Invoke(1);
            _stateService.ChangeState(GameState.Save);
            gameObject.SetActive(false);
        }
    }
}
