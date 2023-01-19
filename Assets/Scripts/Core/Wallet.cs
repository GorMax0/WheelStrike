using System;

namespace Core
{
    public class Wallet
    {
        private const int StartingMoney = 100;

        private int _money;

        public event Action<int> MoneyChanged;
        public event Action<int> MoneyLoaded;

        public int Money => _money;

        public void LoadMoney(int money)
        {            
            _money = money < 0? StartingMoney : money;
            MoneyLoaded?.Invoke(_money);
        }

        public void EnrollMoney(int money)
        {
            if (money <= 0)
                throw new InvalidOperationException($"{GetType()}: EnrollMoney(int money): Amount money {money} is invalid.");

            _money += money;
            MoneyChanged?.Invoke(_money);
        }

        public bool TrySpandMoney(int price)
        {
            if (_money < price)
                return false;

            SpendMoney(price);
            return true;
        }

        private void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException($"{GetType()}: SpendMoney(int money): Amount money {price} is invalid.");

            _money -= price;
            MoneyChanged?.Invoke(_money);
        }
    }
}