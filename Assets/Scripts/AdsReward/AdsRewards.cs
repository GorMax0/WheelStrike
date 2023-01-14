using Core;
using TMPro;
using UnityEngine;

namespace AdsReward
{
    public class AdsRewards : MonoBehaviour
    {
        [SerializeField] private MoneyRewardPanel _moneyRewardPanel;

        private Wallet _wallet;
        private RewardType _rewardType;

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
        }
    }
}