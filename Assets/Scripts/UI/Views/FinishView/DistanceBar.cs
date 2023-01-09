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

        private const int NumberOfSerifs = 5;
        private const int LenghRoadCorrector = 62;

        private FinishViewHandler _viewHandler;
        private float _lengthRoad;
        private float _normalizedDistance;

        private void OnDisable()
        {
            _viewHandler.DisplayedDistanceChanged -= OnDisplayedDistanceChanged;
            _viewHandler.DisplayedHighscoreChanged -= OnDisplayedNewHighscoreLable;
        }

        public void Initialize(FinishViewHandler viewHandler, float lengthRoad)
        {
            _viewHandler = viewHandler;
            _lengthRoad = lengthRoad;
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
        }

        private void SetPointerHighscore(int newHighscore)
        {
            float normalizedDistance = newHighscore / _lengthRoad;
            _highscore.value = normalizedDistance;
        }

        private void ChangeValueSlider(int distanceTraveled)
        {
            _normalizedDistance = distanceTraveled / (_lengthRoad + LenghRoadCorrector);
            _distanceTraveled.value = _normalizedDistance;
        }

        private void OnDisplayedDistanceChanged(int distanceTraveled)
        {
            ChangeValueSlider(distanceTraveled);           
        }

        private void OnDisplayedNewHighscoreLable(int newHighscore)
        {
            SetPointerHighscore(newHighscore);
            Debug.Log("RUN DispalyNewHighscoreLable!");
        }
    }
}