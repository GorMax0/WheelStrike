using System.Collections;

namespace Services.Coroutines
{
    public class CoroutineRunning
    {
        private readonly CoroutineService _coroutineService;
        private readonly int _id;

        public CoroutineRunning(CoroutineService coroutineService)
        {
            _id = coroutineService.GetId();
            _coroutineService = coroutineService;
        }

        public void Run(IEnumerator coroutine)
        {
            _coroutineService.RunSingleCoroutine(_id, coroutine);
        }

        public void Stop()
        {
            _coroutineService.Stop(_id);
        }
    }
}