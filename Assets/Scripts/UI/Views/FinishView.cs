using UnityEngine;
using UnityEngine.Events;

namespace UI.Views
{
    public class FinishView : MonoBehaviour
    {
        public event UnityAction<bool> Enabled;

        private void OnEnable()
        {
            Enabled?.Invoke(true);
        }

        private void OnDisable()
        {
            Enabled?.Invoke(false);
        }
    }
}