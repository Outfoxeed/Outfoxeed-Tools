using System.Collections.Generic;
using UnityEngine;

namespace OutfoxeedTools
{
    public class Pool<T> : IPool where T : MonoBehaviour, IPoolObject
    {
        private T _prefab;
        private Transform _transform;
        private List<T> _activeObjects = new(2);
        private List<T> _availableObjects = new(2);

        public Pool(T prefab, int reservedSize, Transform poolObjectsParent)
        {
            _prefab = prefab;
            _transform = poolObjectsParent;
            _availableObjects.Capacity = reservedSize;
            for (int i = 0; i < reservedSize; i++)
            {
                T instance = CreateInstance();
                _availableObjects.Add(instance);
            }
        }
        
        public T Get()
        {
            T instance = null;
            if (_availableObjects.Count > 0)
            {
                instance = _availableObjects[^1];
                _availableObjects.RemoveAt(_availableObjects.Count - 1);
            }
            else
            {
                instance = CreateInstance();
            }

            _activeObjects.Add(instance);
            instance.PoolObjectReleaseRequested += OnInstanceRequestedRelease;
            instance.gameObject.SetActive(true);
            return instance;
        }

        private void OnInstanceRequestedRelease(IPoolObject poolObject)
        {
            Release(poolObject as T);
        }

        public T Get(Vector3 position)
        {
            T instance = Get();
            instance.transform.position = position;
            return instance;
        }
        
        public T Get(Vector3 position, Quaternion rotation)
        {
            T instance = Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        public void Release(T instance)
        {
            for (int i = _activeObjects.Count - 1; i >= 0; i--)
            {
                if (instance != _activeObjects[i])
                {
                    continue;
                }
                
                instance.PoolObjectReleaseRequested -= OnInstanceRequestedRelease;
                instance.gameObject.SetActive(false);
                
                _activeObjects.RemoveAt(i);
                _availableObjects.Add(instance);
            }
        }

        private T CreateInstance()
        {
            T instance = GameObject.Instantiate(_prefab);
            if (_transform)
            {
                instance.transform.SetParent(_transform);
            }
            instance.gameObject.SetActive(false);
            return instance;
        }
    }
}