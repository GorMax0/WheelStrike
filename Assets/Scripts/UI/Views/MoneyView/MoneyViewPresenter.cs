using UnityEngine;
using Core.Wheel;
using Core;
using UI;
using UI.Views.Money;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class MoneyViewPresenter : MonoBehaviour
{
    [SerializeField] private MoneyView _template;
    [SerializeField] private int _countSpawn = 15;

    private Pool<MoneyView> _pool;
    private MoneyView _view;
    private InteractionHandler _interactionHandler;
    private Coroutine _scatter;
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
        _pool = new Pool<MoneyView>(_countSpawn, _template, transform);

        _isInitialized = true;
        OnEnable();
    }

    public void RunScatter(Vector3 spawnPosition)
    {
        if (_scatter != null)
        {
            StopCoroutine(_scatter);
            _scatter = null;
        }

        StartCoroutine(Scatter(spawnPosition));
    }

    private IEnumerator Scatter(Vector3 spawnPosition)
    {
        WaitForSeconds waitingForStart = new WaitForSeconds(1.8f);
        WaitForSeconds delayBetweenMoney = new WaitForSeconds(0.04f);
        WaitForSeconds delayAnimation = new WaitForSeconds(0.3f);
        List<MoneyView> viewsForFinishView = new List<MoneyView>(_countSpawn);

        yield return waitingForStart;

        for (int i = 0; i < _countSpawn; i++)
        {
            viewsForFinishView.Add(_pool.GetObject());
            viewsForFinishView[i].DisplayOnFinishView(spawnPosition);
            yield return delayBetweenMoney;
        }

        yield return delayAnimation;

        foreach (MoneyView view in viewsForFinishView)
        {
            view.AnimationOnFinishView();
            yield return delayBetweenMoney;
        }
    }

    private void OnCollidedWithObstacle(Obstacle obstacle)
    {
        if (obstacle.IsCollided == true)
            return;

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
