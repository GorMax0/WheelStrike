using UnityEngine;
using Zenject;
using Parameters;

namespace UI.Views
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private ParametrView _template;

        [Inject]
        private void Construct(Parametr[] parametrs)
        {
            foreach (Parametr parametr in parametrs)
            {
                ParametrView view = Instantiate(_template, transform);
                view.Renger(parametr);
            }
        }
    }
}