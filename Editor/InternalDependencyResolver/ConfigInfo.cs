using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OutfoxeedTools.InternalDependencyResolver
{
    public static partial class InternalDependencyResolver
    {
        [System.Serializable]
        private struct ConfigInfo
        {
            public Dictionary<string, string> Dependencies;
        }
    }
}