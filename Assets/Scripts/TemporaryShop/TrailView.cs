using System;
using UnityEngine;
using Trail;

public class TrailView : MonoBehaviour
{
    private TrailFX _trail;
    private bool _isSelected;

    public event Action<TrailFX> Selected;
}
