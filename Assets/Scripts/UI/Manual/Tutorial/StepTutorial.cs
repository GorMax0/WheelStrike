using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Manual.Tutorial
{
    public class StepTutorial : MonoBehaviour
    {
        [SerializeField] private Button _button;

        public event Action StepCompleted;

        public bool IsAction => gameObject.activeInHierarchy;

        public void Enable()
        {
            gameObject.SetActive(true);
            _button?.onClick.AddListener(Complete);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
            _button?.onClick.RemoveListener(Complete);
        }

        private void Complete() => StepCompleted?.Invoke();
    }
}