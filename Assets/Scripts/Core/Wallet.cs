using System;
using System.Linq;
using Parameters;

namespace Core
{
    public class Wallet
    {
        private int _money;
        private Parameter _income;

        public event Action<int> MoneyChanged;

        public Wallet(Parameter[] parameters)
        {
            _income = parameters.Where(parameter => parameter.Name == ParameretName.GetName(ParameterType.Income)).First() 
                ?? throw new NullReferenceException($"{typeof(Wallet)}: Wallet(Parameter[] parameters): {nameof(ParameterType.Income)} is null.");
        }

        public int TemporaryMoney { get; private set; }
        public int BonusMoney { get { return (int)(TemporaryMoney * _income.Value); } }

        public void AddTemporaryMoney(int reward)
        {
            if (reward <= 0)
                throw new InvalidOperationException($"{typeof(Wallet)}: AddTemporaryMoney(int reward): Amount money {reward} is invalid.");

            TemporaryMoney += reward;
        }

        public void EnrollMoney()
        {
            _money += TemporaryMoney + BonusMoney;
            MoneyChanged?.Invoke(_money);
        }

        public void SpendMoney(int price)
        {
            if (price < 0)
                throw new InvalidOperationException($"{typeof(Wallet)}: SpendMoney(int money): Amount money {price} is invalid.");

            _money -= price;
            MoneyChanged?.Invoke(_money);
        }
    }
}