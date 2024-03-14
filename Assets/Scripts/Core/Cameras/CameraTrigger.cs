using System;
using UnityEngine;

namespace Core.Cameras
{
    public class CameraTrigger : MonoBehaviour
    {
        public event Action WheelTriggered;

        public void OnTriggerEnterWheel()
        {
            WheelTriggered?.Invoke();
        }
    }
}