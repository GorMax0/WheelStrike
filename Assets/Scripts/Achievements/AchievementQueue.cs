using System;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
    public class AchievementQueue : MonoBehaviour
    {
        [SerializeField] private AchievementQueueElement _template;

        private List<Achievement> _achievements;
        private readonly Queue<Action> _setsIsDisabled = new Queue<Action>();
        private readonly Queue<AchievementQueueElement> _elements = new Queue<AchievementQueueElement>();

        private AchievementQueueElement _element;

        private void Update()
        {
            DisplayAchievement();
        }

        private void OnDestroy()
        {
            foreach (Achievement achievement in _achievements)
            {
                achievement.AchievementChanged -= OnAchievementChanged;
            }
        }

        public void Initialize(List<Achievement> achievements)
        {
            _achievements = achievements;

            foreach (Achievement achievement in _achievements)
            {
                achievement.AchievementChanged += OnAchievementChanged;
            }
        }

        private void DisplayAchievement()
        {
            if (_element == null || _element.IsPlayAnimation)
                return;

            if (_elements.TryDequeue(out _element))
            {
                _element.StartAnimation();
                Action callback = _setsIsDisabled.Dequeue();
                callback();
            }
        }

        private void RenderElement(Achievement achievement, int currentValue, int targetValue)
        {
            _element = Instantiate(_template, transform);
            _element.Render(achievement);
            _element.gameObject.SetActive(true);
            _element.SetNextValue(currentValue, targetValue);
            _elements.Enqueue(_element);
        }

        private void OnAchievementChanged(
            Achievement achievement,
            int currentValue,
            int targetValue,
            Action setIsDisabled)
        {
            RenderElement(achievement, currentValue, targetValue);
            _setsIsDisabled.Enqueue(setIsDisabled);
        }
    }
}