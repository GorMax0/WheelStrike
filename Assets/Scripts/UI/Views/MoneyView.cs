using TMPro;
using UnityEngine;
using Zenject;
using Core;

namespace UI.Views
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _money;

        private Wallet _wallet;

        private void OnEnable()
        {
            _wallet.MoneyChanged += DisplayAmountOfMoney;
        }

        private void OnDisable()
        {
            _wallet.MoneyChanged -= DisplayAmountOfMoney;
        }

        [Inject]
        private void Construct(Wallet wallet)
        {
            _wallet = wallet;
        }

        private void DisplayAmountOfMoney(int money)
        {
            _money.text = money.ToString();
        }
    }
}