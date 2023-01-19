using System;
using UnityEngine;
using TMPro;
using Core.Wheel;
using Services.Coroutines;
using System.Collections;

namespace UI.Views
{
    public class DistanceView : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _wheelDistance;
        [SerializeField] private TMP_Text _text;

        private CoroutineService _coroutineService;
        private CoroutineRunning _showDistance;
        private bool _isInitialize;

        private ITravelable Travelable => (ITravelable)_wheelDistance;

        private void OnValidate()
        {
            if (_wheelDistance is ITravelable)
                return;

            Debug.LogError(_wheelDistance.name + " needs to implement " + nameof(ITravelable));
            _wheelDistance = null;
        }

        public void Initialize(CoroutineService coroutineService)
        {
            if (_isInitialize == true)
                throw new InvalidOperationException($"{GetType()}: Initialize(CoroutineService coroutineService): Already initialized.");

            _showDistance = new CoroutineRunning(coroutineService);
            _isInitialize = true;
        }

        public void RunShowDistance() => _showDistance.Run(ShowDistance());

        public void StopShowDistance() => _showDistance.Stop();

        private IEnumerator ShowDistance()
        {
            WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
            while (true)
            {
                _text.text = $"{Travelable.DistanceTraveled}m";
                yield return waitForFixedUpdate;
            }
        }
    }
}