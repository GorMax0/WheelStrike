using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [SerializeField] private Transform _distanceOnTable;
    [SerializeField] private TMP_Text _tableText;

    private const int PositionCorrector = 5;

    private void Start()
    {
        RefreshTableText();
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
        RefreshTableText();
    }

    private void RefreshTableText() => _tableText.text = (Mathf.RoundToInt(_distanceOnTable.position.z) * PositionCorrector).ToString();
}
