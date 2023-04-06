using Core;
using Services.GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class DailyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountRewardText;
        [SerializeField] private Button _close;

        private GameStateService _gameStateService;
        private DailyReward _dailyReward;

        private void OnEnable()
        {
            _close.onClick.AddListener(EnrollReward);
        }

        private void OnDisable()
        {
            _close.onClick.RemoveListener(EnrollReward);
        }

        public void Initialize(GameStateService gameStateService, DailyReward dailyReward)
        {
            _gameStateService = gameStateService;
            _gameStateService.GameStateChanged += CheckHasDailyReward;
            _dailyReward = dailyReward;
        }

        private void CheckHasDailyReward(GameState state)
        {
            if (state != GameState.Initializing)
                return;

            Debug.Log($"Invoke Daily!");
            
            if (_dailyReward.HasNextDaily())
                Show();
        }

        private void EnrollReward()
        {
            _dailyReward.EnrollDaily();
            gameObject.SetActive(false);
        }

        private void Show()
        {
            Debug.Log($"Show Daily!");
            gameObject.SetActive(true);
            _amountRewardText.text = _dailyReward.Reward.ToString();
        }
    }
}