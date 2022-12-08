using TMPro;
using UnityEngine;
using Core.Wheel;
public class DistanceView : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _wheelDistance;
    [SerializeField] private TMP_Text _text;

    private ITravelable Travelable => (ITravelable)_wheelDistance;

    private void OnValidate()
    {
        if (_wheelDistance is ITravelable)
            return;

        Debug.LogError(_wheelDistance.name + " needs to implement " + nameof(ITravelable));
        _wheelDistance = null;
    }

    private void Update()
    {
        ShowDistance();
    }

    private void ShowDistance() => _text.text = $"{Travelable.DistanceTraveled}m";
}
