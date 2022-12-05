using System.Collections.Generic;
using UnityEngine;
using Parameters;
using UI.Views;

namespace Core
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParametrView _template;

        private Wallet _wallet;

        public void Initialize(Dictionary<ParameterType, Parameter> parametrs, Wallet wallet)
        {
            foreach (KeyValuePair<ParameterType, Parameter> parametr in parametrs)
            {
                ParametrView view = Instantiate(_template, transform);
                view.Renger(parametr.Value);
            }

            _wallet = wallet;
        }
    }
}