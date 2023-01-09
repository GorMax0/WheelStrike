using System;

namespace Core
{
    public class Wallet
    {
        public event Action<int> MoneyChanged;
        public event Action<int> MoneySpanded;

        public int Money { get; private set; }

        public void LoadMoney(int money)
        {
            if (money < 0)
                return;

            Money = money;
            MoneyChanged?.Invoke(Money);
        }

        public void EnrollMoney(int money)
        {
            if (money <= 0)
                throw new InvalidOperationException($"{GetType()}: EnrollMoney(int money): Amount money {money} is invalid.");

            Money += money;
            MoneyChanged?.Invoke(Money);
        }

        public bool TrySpandMoney(int price)
        {
            if (Money < price)
                return false;

            SpendMoney(price);
            return true;
        }

        private void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException($"{GetType()}: SpendMoney(int money): Amount money {price} is invalid.");

            Money -= price;
            MoneyChanged?.Invoke(Money);
            MoneySpanded?.Invoke(price);
        }
    }
}