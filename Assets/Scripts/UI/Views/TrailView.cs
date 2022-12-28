using UnityEngine;
using UnityEngine.Events;
using Trail;

public class TrailView : MonoBehaviour
{
    private TrailFX _trail;
    private bool _isSelected;

    public event UnityAction<TrailFX> Selected;
}
