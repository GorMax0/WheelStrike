using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class Pool<T> where T : MonoBehaviour
    {
        private T _prefab;
        private Vector3 _position = Vector3.zero;
        private Quaternion _rotation;
        private Transform _container;
        private List<T> _pool;

        public Pool(int countCreateObject, T prefab, Transform container)
        {
            _prefab = prefab;
            _container = container;

            CreatePool(countCreateObject);
        }

        public Pool(int countCreateObject, T prefab, Vector3 position, Quaternion rotation, Transform container)
        {
            _prefab = prefab;
            _position = position;
            _rotation = rotation;
            _container = container;

            CreatePool(countCreateObject);
        }

        public T GetObject() => HasFreeElement(out T element) == true ? element : CreateObject();

        private bool HasFreeElement(out T element) => (element = _pool.Where(T => T.gameObject.activeInHierarchy == false).FirstOrDefault()) != null;

        private T CreateObject()
        {
            T createdObject = Object.Instantiate(_prefab, _position, _rotation, _container);
            createdObject.gameObject.SetActive(false);
            _pool.Add(createdObject);
            return createdObject;
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject();
            }
        }
    }
}