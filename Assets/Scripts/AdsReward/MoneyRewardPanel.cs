using TMPro;
using UnityEngine;

namespace AdsReward
{
    public class MoneyRewardPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countMoneyReward;

        public void DisplayCountMoney(int count)
        {
            _countMoneyReward.text = count.ToString();
            gameObject.SetActive(true);
        }

        public void Disable() => gameObject.SetActive(false);
    }
}