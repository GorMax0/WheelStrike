using System;

namespace Core
{
    public class Wallet
    {
        private int _money;

        public event Action<int> MoneyChanged;

        public void AddMoney(int money)
        {
            if (money < 0)
                throw new ArgumentOutOfRangeException($"{typeof(Wallet)}: AddMoney(int money): Amount money {money} is invalid.");

            _money += money;
            MoneyChanged?.Invoke(_money);
        }

        public void SpendMoney(int money)
        {
            if (money < 0)
                throw new ArgumentOutOfRangeException($"{typeof(Wallet)}: SpendMoney(int money): Amount money {money} is invalid.");

            _money -= money;
            MoneyChanged?.Invoke(_money);
        }
    }
}