using Core;
using GameAnalyticsSDK;
using Parameters;
using Services.GameStates;
using Services.Level;
using UnityEngine;
using UnityEngine.UI;

namespace AdsReward
{
    public class AdsRewards : MonoBehaviour
    {
        [SerializeField] private Button _reward;
        [SerializeField] private LevelService _levelService;
        [SerializeField] private MoneyRewardPanel _moneyRewardPanel;
        [SerializeField] private ParameterRewardPanel _parameterRewardPanel;

        private GameStateService _gameStateService;
        private Wallet _wallet;

        private void OnDisable()
        {
            _reward.onClick.RemoveAllListeners();
        }

        public void Initialize(GameStateService gameStateService, Wallet wallet)
        {
            _gameStateService = gameStateService;
            _wallet = wallet;
        }

        public void EnrollReward(RewardType type, int count = 0)
        {
            switch (type)
            {
                case RewardType.Money:
                    EnrollMoney(count);
                    break;
            }
        }

        public void EnrollParameterLevelUpReward(Parameter parameter, int count)
        {
            gameObject.SetActive(true);
            parameter.LevelUp(count);
            _gameStateService.ChangeState(GameState.Save);
            _parameterRewardPanel.DisplayReward(parameter, count);
            _reward.onClick.RemoveAllListeners();
            _reward.onClick.AddListener(Disable);
        }

        private void EnrollMoney(int count)
        {
            gameObject.SetActive(true);
            _wallet.EnrollMoney(count);
            _gameStateService.ChangeState(GameState.Save);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Money", count, "Reward", "videoAd");
            _moneyRewardPanel.DisplayCountMoney(count);
            _reward.onClick.RemoveAllListeners();
            _reward.onClick.AddListener(_levelService.ShowWorldPanel);
        }

        private void Disable()
        {
            _moneyRewardPanel.Disable();
            _parameterRewardPanel.Disable();
            gameObject.SetActive(false);
        }
    }
}