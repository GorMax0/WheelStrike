using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [field: SerializeField]  public int Reward { get; private set; }

    protected virtual void Encounter()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Encounter();
    }
}
