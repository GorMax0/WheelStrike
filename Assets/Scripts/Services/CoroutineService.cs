using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public  class CoroutineService : MonoBehaviour
{
    private static List<Coroutine> _singelCoroutines;

    public static Coroutine RunSingleCoroutine(Coroutine singelCoroutine)
    {
        Coroutine foundCoroutine = TryFindCoroutine(singelCoroutine);

        if (foundCoroutine != null)
        {
            
        }

        return null;
    }

    private static Coroutine TryFindCoroutine(Coroutine singleCoroutine) => _singelCoroutines.Where(coroutine => coroutine == singleCoroutine).FirstOrDefault();
}
