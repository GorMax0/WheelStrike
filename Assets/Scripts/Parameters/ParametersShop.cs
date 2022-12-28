using System.Collections.Generic;
using UnityEngine;
using Core;
using UI.Views;

namespace Parameters
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParameterView _template;

        private Dictionary<ParameterType, ParameterView> _views = new Dictionary<ParameterType, ParameterView>();
        private Wallet _wallet;

        public void Initialize(Dictionary<ParameterType, Parameter> parametrs, Wallet wallet)
        {
            foreach (KeyValuePair<ParameterType, Parameter> parametr in parametrs)
            {
                ParameterView view = Instantiate(_template, transform);
                view.LevelUpButtonClicked += OnLevelUpButtonClicked;
                view.Renger(parametr.Value);
                view.SubscribeToLevelChange();
                _views.Add(parametr.Key, view);
            }

            _wallet = wallet;
        }

        private void TryParameterLevelUp(Parameter parameter)
        {
            if (_wallet.Money < parameter.Cost)
                return;

            _wallet.SpendMoney(parameter.Cost);
            parameter.LevelUp();
        }

        private void OnLevelUpButtonClicked(Parameter parameter)
        {
            TryParameterLevelUp(parameter);
        }
    }
}