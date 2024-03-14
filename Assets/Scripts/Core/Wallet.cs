using System;

namespace Core
{
    public class Wallet
    {
        public int Money { get; private set; }

        public event Action<int> MoneyChanged;

        public event Action<int> MoneyLoaded;

        public void LoadMoney(int money)
        {
            if (money < 0)
                return;

            Money = money;
            MoneyLoaded?.Invoke(Money);
        }

        public void EnrollMoney(int money)
        {
            if (money <= 0)
                throw new InvalidOperationException(
                    $"{GetType()}: EnrollMoney(int money): Amount money {money} is invalid.");

            Money += money;
            MoneyChanged?.Invoke(Money);
        }

        public bool TrySpendMoney(int price)
        {
            if (Money < price)
                return false;

            SpendMoney(price);

            return true;
        }

        public void Reset()
        {
            Money = 100;
            MoneyChanged?.Invoke(Money);
        }

        private void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException(
                    $"{GetType()}: SpendMoney(int money): Amount money {price} is invalid.");

            Money -= price;
            MoneyChanged?.Invoke(Money);
        }
    }
}