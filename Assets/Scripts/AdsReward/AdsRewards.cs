using System;
using Core;
using GameAnalyticsSDK;
using Parameters;
using UI.Views;
using UnityEngine;
using UnityEngine.UI;

namespace AdsReward
{
    public class AdsRewards : MonoBehaviour
    {
        [SerializeField] private Button _reward;
        [SerializeField] private TopPanel _topPanel;
        [SerializeField] private MoneyRewardPanel _moneyRewardPanel;
        [SerializeField] private ParameterRewardPanel _parameterRewardPanel;

        private Wallet _wallet;

        private void OnDisable()
        {
            _reward.onClick.RemoveAllListeners();
        }

        public void Initialize(Wallet wallet)
        {
            _wallet = wallet;
        }

        public void Disable()
        {
            _moneyRewardPanel.Disable();
            _parameterRewardPanel.Disable();
            gameObject.SetActive(false);
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
            parameter.LevelUp(count);
            _parameterRewardPanel.DisplayReward(parameter, count);
            _reward.onClick.RemoveAllListeners();
            _reward.onClick.AddListener(Disable);
            gameObject.SetActive(true);
        }

        private void EnrollMoney(int count)
        {
            gameObject.SetActive(true);
            _wallet.EnrollMoney(count);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Money", count, "Reward", "videoAd");
            _moneyRewardPanel.DisplayCountMoney(count);
            _reward.onClick.RemoveAllListeners();
            _reward.onClick.AddListener(_topPanel.Restart);
        }
    }
}