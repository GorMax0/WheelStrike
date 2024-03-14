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
        [SerializeField] private GameObject _rewardView;
        [SerializeField] private GameObject _rewardErrorView;
        [SerializeField] private Button _rewardEnroll;
        [SerializeField] private Button _rewardError;
        [SerializeField] private LevelService _levelService;
        [SerializeField] private MoneyRewardPanel _moneyRewardPanel;
        [SerializeField] private ParameterRewardPanel _parameterRewardPanel;

        private GameStateService _gameStateService;
        private Wallet _wallet;

        private void OnDisable()
        {
            _rewardView.SetActive(false);
            _rewardErrorView.SetActive(false);
            _rewardEnroll.onClick.RemoveAllListeners();
            _rewardError.onClick.RemoveAllListeners();
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
            _rewardView.SetActive(true);
            parameter.LevelUp(count);
            _gameStateService.ChangeState(GameState.Save);
            _parameterRewardPanel.DisplayReward(parameter, count);
            _rewardEnroll.onClick.RemoveAllListeners();
            _rewardEnroll.onClick.AddListener(Disable);
        }

        public void ShowErrorAds()
        {
            gameObject.SetActive(true);
            _rewardErrorView.SetActive(true);
            _rewardError.onClick.AddListener(Disable);
        }

        private void EnrollMoney(int count)
        {
            gameObject.SetActive(true);
            _rewardView.SetActive(true);
            _wallet.EnrollMoney(count);
            _gameStateService.ChangeState(GameState.Save);
            GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Money", count, "Reward", "videoAd");
            _moneyRewardPanel.DisplayCountMoney(count);
            _rewardEnroll.onClick.RemoveAllListeners();
            _rewardEnroll.onClick.AddListener(_levelService.ShowWorldPanel);
        }

        private void Disable()
        {
            _moneyRewardPanel.Disable();
            _parameterRewardPanel.Disable();
            gameObject.SetActive(false);
        }
    }
}