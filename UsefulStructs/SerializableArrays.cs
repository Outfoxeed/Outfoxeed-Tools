using UnityEngine;

namespace OutFoxeedTools.UsefulStructs
{
    /// <summary>
    /// Serializable struct representing a 2D array with object of type T.
    /// Meant to represent 2D arrays in Unity inspector
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public struct Array2D<T> where T : UnityEngine.Object
    {
        [SerializeField] private Array<T>[] arrays;
        public int Length => arrays.Length;
        public int TotalLength()
        {
            var totalLength = 0;
            for (int i = 0; i < Length; i++)
            {
                totalLength += this[i].Length;
            }
            return totalLength;
        }
        public Array<T> this[int index] => arrays[index];
    }
    
    /// <summary>
    /// Class containing an array of objects of type T. Meant to only be used in the Array2D struct
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public struct Array<T> where T : UnityEngine.Object
    {
        [SerializeField] private T[] items;
        public int Length => items.Length;
        public T this[int index] => items[index];
    }
}
