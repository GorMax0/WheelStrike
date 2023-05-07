using UnityEngine;
using UnityEngine.UI;

namespace Achievements
{
    public class AchievementQueueElement : AchievementElement
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _starFill;
        
        private readonly int StartAnimationTrigger = Animator.StringToHash("StartAnimation");
        private readonly int StarScale = Animator.StringToHash("Scale");

        private int _currentValue;
        private bool _isSliderStarted;

        public bool IsPlayAnimation { get; private set; }

        protected override void OnEnable()
        {
            
        }

        public void StartAnimation()
        {
            _animator.SetTrigger(StartAnimationTrigger);
            IsPlayAnimation = true;
        }

        public void SetNextValue(int currentValue, int endValue)
        {
            _currentValue = currentValue < endValue ? currentValue : endValue;
            NextValue = endValue;
            GetValues();
            FillStars();
        }
        
        protected override void FillStars()
        {
            for (int i = 0; i < CountStars; i++)
            {
                if (i < Achievement.CountAchieved)
                {
                  var star =  Instantiate(_starFill, Stars[i].transform);
                  star.GetComponent<Animator>().SetTrigger(StarScale);
                }
            }
        }

        protected override void GetValues()
        {
            TextValue.SetText($"{_currentValue}/{NextValue}");
        }

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