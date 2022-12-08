using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Finish
{
    public class DistanceBar : MonoBehaviour
    {
        [SerializeField] private Slider _distanceTraveled;
        [SerializeField] private TMP_Text[] _serifs;

        private const int NumberOfSerifs = 5;

        private FinishViewHandler _viewHandler;
        private float _lengthRoad;
        private float _normalizedDistance;

        private void OnEnable()
        {
            _viewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
        }

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
        }

        public void Initialize(FinishViewHandler viewHandler, float lengthRoad)
        {
            _viewHandler = viewHandler;
            _lengthRoad = lengthRoad;
            SetValueForSerifs();
        }

        private void SetValueForSerifs()
        {
            int valueOneSerif = (int)_lengthRoad / NumberOfSerifs;
            int valueNextSerif;

            for (int i = 0; i < _serifs.Length; i++)
            {
                valueNextSerif = valueOneSerif * (i + 1);
                _serifs[i].text = $"{valueNextSerif}";
            }
        }

        private void ChangeValueSlider(int distanceTraveled)
        {
            _normalizedDistance = distanceTraveled / _lengthRoad;
            _distanceTraveled.value = _normalizedDistance;
        }

        private void OnDisplayedDistanceChanged(int distanceTraveled)
        {
            ChangeValueSlider(distanceTraveled);
        }
    }
}