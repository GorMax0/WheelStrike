using TMPro;
using UnityEngine;
using Core;

namespace UI.Views.Money
{
    public class WalletView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;

        private Wallet _wallet;

        private void OnEnable()
        {
            if (_wallet == null)
                return;

            _wallet.MoneyLoaded += DisplayAmountOfMoney;
            _wallet.MoneyChanged += DisplayAmountOfMoney;
        }

        private void OnDisable()
        {
            _wallet.MoneyLoaded -= DisplayAmountOfMoney;
            _wallet.MoneyChanged -= DisplayAmountOfMoney;
        }

        public void Initialize(Wallet wallet)
        {
            if (_wallet != null)
                return;

            _wallet = wallet;
            OnEnable();
        }

        private void DisplayAmountOfMoney(int money) => _money.text = money.ToString();
    }
}