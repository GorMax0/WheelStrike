using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Parameters;
using UnityEngine.Events;

namespace UI.Views
{
    public class ParameterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _cost;
        [SerializeField] private Image _icon;
        [SerializeField] private Button _levelUp;

        private Parameter _parametr;

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
        }

        public void SubscribeToLevelChange()
        {
            _parametr.Loaded += Refresh;  // Где нужно отписаться?
        }

        private void OnButtonClick()
        {
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