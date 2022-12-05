using System;

namespace Core
{
    public class Wallet
    {
        private int _money;

        public event Action<int> MoneyChanged;

        public void EnrollMoney(int money)
        {
            if (money <= 0)
                throw new InvalidOperationException($"{GetType()}: EnrollMoney(int money): Amount money {money} is invalid.");

            _money += money;
            MoneyChanged?.Invoke(_money);
        }

        public void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException($"{GetType()}: SpendMoney(int money): Amount money {price} is invalid.");

            _money -= price;
            MoneyChanged?.Invoke(_money);
        }
    }
}