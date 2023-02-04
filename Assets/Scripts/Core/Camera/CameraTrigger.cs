using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public event Action WheelTriggered;

    public void OnTriggerEnterWheel()
    {
        WheelTriggered?.Invoke();
    }
}
