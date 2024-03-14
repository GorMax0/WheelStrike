using TMPro;
using UnityEngine;

namespace Core
{
    public class Tile : MonoBehaviour
    {
        private const int PositionCorrector = 5;
        [SerializeField] private Transform _distanceOnTable;
        [SerializeField] private TMP_Text _tableText;

        private void Start()
        {
            RefreshTableText();
        }

        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
            RefreshTableText();
        }

        private void RefreshTableText() =>
            _tableText.text = (Mathf.RoundToInt(_distanceOnTable.position.z) * PositionCorrector).ToString();
    }
}