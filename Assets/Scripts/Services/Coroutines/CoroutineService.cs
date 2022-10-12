using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Coroutines
{
    public class CoroutineService : MonoBehaviour
    {
        private int _ids = 0;
        private Dictionary<int, Coroutine> _coroutines = new Dictionary<int, Coroutine>();

        public int GetId() => ++_ids;

        public void RunSingleCoroutine(int id, IEnumerator coroutine)
        {
            if (_coroutines.ContainsKey(id) == false)
                _coroutines.Add(id, null);

            Stop(id);

            _coroutines[id] = StartCoroutine(coroutine);
        }

        public void Stop(int id)
        {
            if (_coroutines.ContainsKey(id) == false)
                return;

            Coroutine job = _coroutines[id];

            if (job != null)
                StopCoroutine(job);
        }
    }
}