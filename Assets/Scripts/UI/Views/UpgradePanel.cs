using UnityEngine;
using Parameters;

namespace UI.Views
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private ParametrView _template;

        public void Initialize(Parametr[] parametrs)
        {
            foreach (Parametr parametr in parametrs)
            {
                ParametrView view = Instantiate(_template, transform);
                view.Renger(parametr);
            }
        }
    }
}