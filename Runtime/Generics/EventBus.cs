using System;

namespace OutfoxeedTools
{
    public static class EventBus<T> where T : struct
    {
        private static event Action<T> Callback = null;
        
        public static void Raise(T eventValue)
        {
            Callback?.Invoke(eventValue);
        }

        public static void Register(Action<T> callback)
        {
            Callback += callback;
        }

        public static void Unregister(Action<T> callback)
        {
            Callback -= callback;
        }

        public static void Clear()
        {
            Callback = null;
        }
    }
}