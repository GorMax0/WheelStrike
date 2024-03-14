using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class WorldPanel : MonoBehaviour
    {
        private const float EndScaleStar = 1f;
        private const float DurationAnimation = 0.5f;
        [SerializeField] private Image _currentLevelStar;
        [SerializeField] private Image _hideNextLocation;
        [SerializeField] private Button _nextLevel;
        [SerializeField] private Button _close;
        [SerializeField] private AnimationCurve _displayStar;
        [SerializeField] private TopPanel _topPanel;

        private void OnEnable()
        {
            _nextLevel.onClick.AddListener(_topPanel.Restart);
        }

        private void OnDisable()
        {
            _nextLevel.onClick.RemoveListener(_topPanel.Restart);
        }

        public void DisplayProgress()
        {
            _close.gameObject.SetActive(false);
            _hideNextLocation.gameObject.SetActive(false);
            _currentLevelStar.gameObject.SetActive(true);

            _currentLevelStar.transform.DOScale(EndScaleStar, DurationAnimation)
                .ChangeStartValue(Vector3.zero)
                .SetEase(_displayStar);

            _nextLevel.gameObject.SetActive(true);
        }
    }
}