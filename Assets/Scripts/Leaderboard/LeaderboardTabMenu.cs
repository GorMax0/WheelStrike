using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Leaderboards
{
    public class LeaderboardTabMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _distanceTabText;
        [SerializeField] private TMP_Text _collisionTabText;
        [SerializeField] private Color _unselectedColor;
        [SerializeField] private Image _focusDistanceTab;
        [SerializeField] private Image _focusCollisionTab;

        private Color _selectedColor = Color.white;
        private bool _selectedCollisionTab;

        public event System.Action<bool> CollisionTabSelected;

        public void SelectCollisionTab(bool isSelected)
        {
            _selectedCollisionTab = isSelected;

            _focusDistanceTab.gameObject.SetActive(!_selectedCollisionTab);
            _distanceTabText.color = _selectedCollisionTab ? _unselectedColor : _selectedColor;

            _focusCollisionTab.gameObject.SetActive(_selectedCollisionTab);
            _collisionTabText.color = _selectedCollisionTab ? _selectedColor : _unselectedColor;

            CollisionTabSelected?.Invoke(_selectedCollisionTab);
        }
    }
}