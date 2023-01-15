using Core;
using TMPro;
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

        private Wallet _wallet;

        private void OnDisable()
        {
            _reward.onClick.RemoveListener(_topPanel.Restart);
        }

        public void Initialize(Wallet wallet)
        {
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

        private void EnrollMoney(int count)
        {
            _wallet.EnrollMoney(count);
            _moneyRewardPanel.DisplayCountMoney(count);
            _reward.onClick.AddListener(_topPanel.Restart);
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}