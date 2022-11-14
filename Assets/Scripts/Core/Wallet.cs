using System;

namespace Core
{
    public class Wallet
    {
        private int _money;
        private int _credit;

        public event Action<int> MoneyChanged;

        public void AddCredit(int reward)
        {
            if (reward <= 0)
                throw new InvalidOperationException($"{typeof(Wallet)}: AddCredit(int reward): Amount money {reward} is invalid.");

            _credit += reward;
        }

        public void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException($"{typeof(Wallet)}: SpendMoney(int money): Amount money {price} is invalid.");

            _money -= price;
            MoneyChanged?.Invoke(_money);
        }

        private void AddMoney(int reward)
        {
            if (reward <= 0)
                throw new InvalidOperationException($"{typeof(Wallet)}: AddMoney(int money): Amount money {reward} is invalid.");

            _money += reward;
            MoneyChanged?.Invoke(_money);
        }
    }
}