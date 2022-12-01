using UnityEngine;
using Parameters;
using UI.Views;

namespace Core
{
    public class ParametersShop : MonoBehaviour
    {
        [SerializeField] private ParametrView _template;

        private Wallet _wallet;

        public void Initialize(Parameter[] parametrs, Wallet wallet)
        {
            foreach (Parameter parametr in parametrs)
            {
                ParametrView view = Instantiate(_template, transform);
                view.Renger(parametr);
            }

            _wallet = wallet;
        }
    }
}