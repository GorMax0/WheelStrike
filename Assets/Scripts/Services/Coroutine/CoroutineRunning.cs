using System.Collections;

public class CoroutineRunning
{
    private int _id;
    private CoroutineService _coroutineService;

    public CoroutineRunning(CoroutineService coroutineService)
    {
        _id = coroutineService.GetId();
        _coroutineService = coroutineService;
    }

    public void Run(IEnumerator coroutine)
    {
        _coroutineService.RunSingleCoroutine(_id, coroutine);
    }
}
