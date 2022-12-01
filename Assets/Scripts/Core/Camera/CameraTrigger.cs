using Core.Wheel;
using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour
{
    public event UnityAction WheelTriggered;

    public void OnTriggerEnterWheel()
    {
        WheelTriggered?.Invoke();
    }
}
