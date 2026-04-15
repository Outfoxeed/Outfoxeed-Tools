using System;

namespace OutfoxeedTools
{
    public interface IPoolObject
    {
        public event Action<IPoolObject> PoolObjectReleaseRequested;
    }
}