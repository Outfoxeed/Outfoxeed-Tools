using System;
using System.Collections.Generic;
using UnityEngine;

namespace OutfoxeedTools
{
    public class Provider : MonoBehaviour
    {
        private static readonly Dictionary<Type, MonoBehaviour> _singletons = new();
        private static readonly Dictionary<Type, GlobalData> _globalData = new();
        
        [SerializeField] private GlobalData[] _globalDataEntries;

        private void Awake()
        {
            _singletons.Clear();
            _globalData.Clear();
            foreach (GlobalData globalDataEntry in _globalDataEntries)
            {
                _globalData.Add(globalDataEntry.GetType(), globalDataEntry);
            }
        }

        public static T GetSingleton<T>() where T : MonoBehaviour
        {
            if (_singletons.TryGetValue(typeof(T), out MonoBehaviour value) && value)
            {
                return (T)value;
            }
            
            T firstFoundInstance = FindFirstObjectByType<T>();
            if (firstFoundInstance != null)
            {
                _singletons.Add(typeof(T), firstFoundInstance);
            }
            
            return firstFoundInstance;
        }

        public static T GetGlobalData<T>() where T : GlobalData
        {
            if (_globalData.TryGetValue(typeof(T), out GlobalData value) && value)
            {
                return (T)value;
            }

            Debug.LogWarning($"No GlobalData found of type '{typeof(T).FullName}'");
            return null;
        }
    }
}