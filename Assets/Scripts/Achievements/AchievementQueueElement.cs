using UnityEngine;

namespace Achievements
{
    public class AchievementQueueElement : AchievementElement
    {
        [SerializeField] private Animator _animator;

        private readonly int StartAnimationTrigger = Animator.StringToHash("StartAnimation");

        private int _currentValue;
        private bool _isSliderStarted;

        public bool IsPlayAnimation { get; private set; }

        protected override void OnEnable()
        {
            FillStars();
        }

        public void StartAnimation()
        {
            _animator.SetTrigger(StartAnimationTrigger);
            IsPlayAnimation = true;
        }

        public void SetNextValue(int currentValue, int endValue)
        {
            _currentValue = currentValue;
            NextValue = endValue;
            GetValues();
        }

        protected override void GetValues() =>
            TextValue.SetText($"{_currentValue}/{NextValue}");

        protected override float GetNormalizedBarValue() =>
            NextValue / (float)NextValue;

        private void FinishAnimation()
        {
            IsPlayAnimation = false;
            gameObject.SetActive(false);
        }

        private void StartProgressSlider() =>
            ProgressSlider.SetNormalizedValue(GetNormalizedBarValue());
    }
}