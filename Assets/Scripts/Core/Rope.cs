using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private RopePoint[] _ropePoints;
    
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        
       for (int i = 0; i < _ropePoints.Length; i++)
        {
            _lineRenderer.SetPosition(i,_ropePoints[i].transform.position);
        }
    }
}
