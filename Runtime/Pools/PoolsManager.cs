using System.Collections.Generic;
using UnityEngine;

namespace OutfoxeedTools
{
    public class PoolsManager : MonoBehaviour
    {
        private const int DefaultPoolReservedSize = 16;
        
        private Dictionary<MonoBehaviour, IPool> _pools = new();
        
        public Pool<T> GetPool<T>(T prefab) where T : MonoBehaviour, IPoolObject
        {
            if (_pools.TryGetValue(prefab, out IPool pool))
            {
                return (Pool<T>)pool;
            }

#if UNITY_EDITOR
            Transform newPoolTransform = new GameObject($"Pool '{prefab.gameObject.name}'").transform;
            newPoolTransform.SetParent(transform);
            Pool<T> newPool = new Pool<T>(prefab, DefaultPoolReservedSize, newPoolTransform);
#else
            Pool<T> newPool = new Pool<T>(prefab, DefaultPoolReservedSize, null);
#endif
            _pools.Add(prefab, newPool);
            return newPool;
        }
    }
}