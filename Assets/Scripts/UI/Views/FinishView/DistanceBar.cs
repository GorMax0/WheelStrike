using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Finish
{
    public class DistanceBar : MonoBehaviour
    {
        [SerializeField] private Slider _distanceTraveled;
        [SerializeField] private Slider _highscore;
        [SerializeField] private TMP_Text[] _serifs;

        private const float StartSliderValue = 0f;
        private const int NumberOfSerifs = 5;
        private const float LenghRoadCorrector = 0.16f;

        private FinishViewHandler _viewHandler;
        private float _lengthRoad;
        private float _normalizedDistance;

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedHighscoreChanged -= OnDisplayedNewHighscoreLable;
            _viewHandler.DisplayedHighscoreLoaded -= OnDisplayedNewHighscoreLable;
        }

        public void Initialize(FinishViewHandler viewHandler, float lengthRoad)
        {
            _viewHandler = viewHandler;
            _lengthRoad = lengthRoad;
            _distanceTraveled.value = StartSliderValue;
            _highscore.value = StartSliderValue;
            SetValueForSerifs();
            Subscribe();
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

        private void Subscribe()
        {
            _viewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
            _viewHandler.DisplayedHighscoreChanged += OnDisplayedNewHighscoreLable;
            _viewHandler.DisplayedHighscoreLoaded += OnDisplayedNewHighscoreLable;
        }

        private void ChangeValueSlider(Slider slider, int ValueForNormalization)
        {
            _normalizedDistance = ValueForNormalization / (_lengthRoad + LenghRoadCorrector * _lengthRoad);
            slider.value = _normalizedDistance;
        }

        private void OnDisplayedDistanceChanged(int distanceTraveled)
        {
            ChangeValueSlider(_distanceTraveled, distanceTraveled);
        }

        private void OnDisplayedNewHighscoreLable(int newHighscore)
        {
            ChangeValueSlider(_highscore, newHighscore);
        }
    }
}