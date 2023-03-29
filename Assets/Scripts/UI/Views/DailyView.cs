using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class DailyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountRewardText;
        [SerializeField] private Button _close;

        private DailyReward _dailyReward;

        private void OnEnable()
        {
            _close.onClick.AddListener(EnrollReward);
        }

        private void OnDisable()
        {
            _close.onClick.RemoveListener(EnrollReward);
        }

        public void Initialize(DailyReward dailyReward)
        {
            _dailyReward = dailyReward;

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
            gameObject.SetActive(true);
            _amountRewardText.text = _dailyReward.Reward.ToString();
        }
    }
}