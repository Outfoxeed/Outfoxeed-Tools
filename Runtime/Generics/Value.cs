using System;
using System.Collections.Generic;
using UnityEngine;

namespace OutfoxeedTools
{
    [Serializable]
    public class Value<T> : IEquatable<Value<T>>
    {
        public event Action<T> ValueChanged;
        [SerializeField, ReadOnly]
        private T _value;

        public T Get() => _value;
        public void Set(T value)
        {
            _value = value;
            ValueChanged?.Invoke(value);
        }
        
        public void Reset()
        {
            _value = default;
            ValueChanged = null;
        }

        public static implicit operator T(Value<T> value) => value.Get();
        
        public bool Equals(Value<T> other)
        {
            return EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Value<T> other && Equals(other);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
        }
    }
}