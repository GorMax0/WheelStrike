using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboard
{
    [RequireComponent(typeof(Button))]
    public class LeaderboardTab : MonoBehaviour
    {
        [SerializeField] private LeaderboardType _type;
        [SerializeField] private LeaderboardView _view;
        [SerializeField] private Image _focusImage;
        [SerializeField] private TMP_Text _text;

        private Button _button;

        public event Action<LeaderboardType> ButtonClicked;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ClickButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ClickButton);
        }

        public void Enable(Color colorText)
        {
            _view.gameObject.SetActive(true);
            _focusImage.gameObject.SetActive(true);
            _text.color = colorText;
        }

        public void Disable(Color colorText)
        {
            _view.gameObject.SetActive(false);
            _focusImage.gameObject.SetActive(false);
            _text.color = colorText;
        }

        private void ClickButton() => ButtonClicked?.Invoke(_type);
    }
}