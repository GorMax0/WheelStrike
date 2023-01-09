using UnityEngine;
using Core.Wheel;
using Core;
using UI;
using UI.Views.Money;

public class MoneyViewPresenter : MonoBehaviour
{
    [SerializeField] private MoneyView _template;
    [SerializeField] private int _countCreateObject = 7;

    private Pool<MoneyView> _pool;
    private MoneyView _view;
    private InteractionHandler _interactionHandler;
    private bool _isInitialized;

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _interactionHandler.CollidedWithObstacle += OnCollidedWithObstacle;
        _interactionHandler.TriggeredEnterWithCar += OnTriggeredEnterWithCar;
    }

    private void OnDisable()
    {
        _interactionHandler.CollidedWithObstacle -= OnCollidedWithObstacle;
        _interactionHandler.TriggeredEnterWithCar -= OnTriggeredEnterWithCar;
    }

    public void Initialize(InteractionHandler interactionHandler)
    {
        if (_isInitialized == true)
            return;

        _interactionHandler = interactionHandler;
        _pool = new Pool<MoneyView>(_countCreateObject, _template, transform);

        _isInitialized = true;
        OnEnable();
    }

    private void OnCollidedWithObstacle(Obstacle obstacle)
    {
        _view = _pool.GetObject();
        _view.SetTransformParameters(obstacle.transform.position);
        _view.Display(obstacle.Reward);
    }

    private void OnTriggeredEnterWithCar(Car car)
    {
        _view = _pool.GetObject();
        _view.SetTransformParameters(car.transform.position);
        _view.Display(car.Reward);
    }
}
