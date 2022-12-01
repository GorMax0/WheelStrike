using System;
using UnityEngine;

namespace UI.Views.Finish
{
    public class FacadeFinishView : MonoBehaviour
    {
        [SerializeField] private FinishView[] _finishViews;

        private FinishViewHandler _finishViewHandler;
        private bool _isInitialized;

        private void OnEnable()
        {
            if (_isInitialized == false)
                return;

            _finishViewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
        }

        private void OnDisable()
        {
            _finishViewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
        }

        public void Initialized(FinishViewHandler finishViewHandler)
        {
            if (_isInitialized == true)
                throw new InvalidOperationException($"{GetType()}: Initialized(FinishViewHandler finishViewHandler) : Already initialized.");

            _finishViewHandler = finishViewHandler;
            _isInitialized = true; 
            OnEnable();
        }

        private void OnDisplayedDistanceChanged(float distance)
        {
            foreach (FinishView view in _finishViews)
            {
                view.ShowDistance((int)distance);
            }
        }
    }
}