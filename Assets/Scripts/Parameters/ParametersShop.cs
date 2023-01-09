using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.Wheel;
using UI.Views;

namespace Parameters
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParameterView _template;
        [SerializeField] private AnimationWheel _animationWheel;

        private Dictionary<ParameterType, ParameterView> _views = new Dictionary<ParameterType, ParameterView>();
        private Wallet _wallet;

        public void Initialize(Dictionary<ParameterType, Parameter> parametrs, Wallet wallet)
        {
            foreach (KeyValuePair<ParameterType, Parameter> parametr in parametrs)
            {
                ParameterView view = Instantiate(_template, transform);
                view.LevelUpButtonClicked += OnLevelUpButtonClicked;  // Где должна быть отписка?
                view.Renger(parametr.Value);
                view.SubscribeToLevelChange();
                _views.Add(parametr.Key, view);
            }

            _wallet = wallet;
        }

        private void TryParameterLevelUp(Parameter parameter)
        {
            if (_wallet.TrySpandMoney(parameter.Cost) == false)
                return;

            parameter.LevelUp();
            _animationWheel.ParameterUp();
        }

        private void OnLevelUpButtonClicked(Parameter parameter)
        {
            TryParameterLevelUp(parameter);
        }
    }
}