using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Finish
{
    public class DistanceBar : MonoBehaviour
    {
        private const float StartSliderValue = 0f;
        private const int NumberOfSerifs = 5;
        private const float LenghRoadCorrector = 0.16f;
        [SerializeField] private Slider _distanceTraveled;
        [SerializeField] private Slider _highscore;
        [SerializeField] private TMP_Text[] _serifs;

        private FinishViewHandler _viewHandler;
        private int _highscoreValue;
        private float _lengthRoad;
        private float _normalizedDistance;
        private bool _isSubscribe;

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedHighscoreChanged -= OnDisplayedNewHighscoreLable;
            _viewHandler.DisplayedHighscoreLoaded -= OnDisplayedNewHighscoreLable;
            _isSubscribe = false;
        }

        public void Initialize(FinishViewHandler viewHandler, float lengthRoad)
        {
            _viewHandler = viewHandler;
            _lengthRoad = lengthRoad;
            _distanceTraveled.value = StartSliderValue;
            ChangeValueSlider(_highscore, _highscoreValue);
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
            if (_isSubscribe)
                return;

            _viewHandler.DisplayedDistanceChanged += OnDisplayedDistanceChanged;
            _viewHandler.DisplayedHighscoreChanged += OnDisplayedNewHighscoreLable;
            _viewHandler.DisplayedHighscoreLoaded += OnDisplayedNewHighscoreLable;
            _isSubscribe = true;
        }

        private void ChangeValueSlider(Slider slider, int valueForNormalization)
        {
            _normalizedDistance = valueForNormalization / (_lengthRoad + LenghRoadCorrector * _lengthRoad);
            slider.value = _normalizedDistance;
        }

        private void OnDisplayedDistanceChanged(int distanceTraveled) =>
            ChangeValueSlider(_distanceTraveled, distanceTraveled);

        private void OnDisplayedNewHighscoreLable(int newHighscore) => _highscoreValue = newHighscore;
    }
}