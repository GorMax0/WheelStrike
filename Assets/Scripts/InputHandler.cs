using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Zenject;

public class InputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private GameStateService _gameStateService;

    public event UnityAction PointerDown;
    public event UnityAction PointerUp;

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUp?.Invoke();
    }

    [Inject]
    private void Construct(GameStateService gameStateService)
    {
        _gameStateService = gameStateService;
    }
}
