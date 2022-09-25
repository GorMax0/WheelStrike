using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ParametrView : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _cost;
    [SerializeField] private Image _icon;

    private Parametr _parametr;

    public void Renger(Parametr parametr)
    {
        _parametr = parametr;
        _name.text = _parametr.Name;       
        _level.text = $"Level {_parametr.Level}";
        _cost.text = _parametr.Cost.ToString();
        _icon.sprite = _parametr.Icon;
    }
}
