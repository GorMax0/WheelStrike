using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StartupPower : MonoBehaviour
{
    private float _power;
    private Coroutine _changePower;

    private void Start()
    {
        
    }

    private void StartCoroutine()
    {
        if( _changePower != null )
            StopCoroutine(_changePower);

   //     _changePower = StartCoroutine()
    }

    //private IEnumerator ChangePower()
    //{
    //    while ()
    //}
}
