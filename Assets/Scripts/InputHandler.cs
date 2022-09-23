using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    public event UnityAction MouseButtonUp;
    public event UnityAction MouseButtonDown;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            MouseButtonDown?.Invoke();

        if(Input.GetMouseButtonUp(0))
            MouseButtonUp?.Invoke();
    }
}
