using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Parameters;
using UnityEngine.Events;
using DG.Tweening;

namespace UI.Views
{
    public class ParameterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _levelUp;

        private const float ScaleRatio = 1.1f;
        private const float AnimationDuration = 0.07f;
        
        private Parameter _parametr;
        private Vector2 _startScale;
        private Vector2 _scaleIncrease;

        public event UnityAction<Parameter> LevelUpButtonClicked;

        private void OnEnable()
        {
            _levelUp.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _levelUp.onClick.RemoveListener(OnButtonClick);
        }

        public void Renger(Parameter parametr)
        {
            _parametr = parametr;
            _name.text = _parametr.Name;
            _level.text = $"Ур. {_parametr.Level}";
            _cost.text = _parametr.Cost.ToString();
            _icon.sprite = _parametr.Icon;
            _startScale = _levelUp.transform.localScale;
            _scaleIncrease = _startScale * ScaleRatio;
        }

        public void SubscribeToLevelChange()
        {
            _parametr.Loaded += Refresh;  // Где нужно отписаться?
        }

        private void OnButtonClick()
        {
            DOTween.Sequence()
                .Append(_levelUp.transform.DOScale(_scaleIncrease, AnimationDuration)).SetEase(Ease.InOutQuad)
                .Append(_levelUp.transform.DOScale(_startScale, AnimationDuration));

            LevelUpButtonClicked?.Invoke(_parametr);
            Refresh();
        }

        private void Refresh()
        {
            _level.text = $"Ур. {_parametr.Level}";
            _cost.text = _parametr.Cost.ToString();
        }
    }
}