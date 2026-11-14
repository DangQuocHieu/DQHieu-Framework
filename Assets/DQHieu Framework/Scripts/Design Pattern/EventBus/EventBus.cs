namespace DQHieu.Framework.DesignPattern
{
    using System;
    public static class EventBus<T> where T : IGameEvent
    {
        private static Action<T> onEvent;

        public static void Subscribe(Action<T> listener)
        {
            onEvent += listener;
        }

        public static void Unsubscribe(Action<T> listener)
        {
            onEvent -= listener;
        }

        public static void Raise(T eventData)
        {
            onEvent?.Invoke(eventData);
        }
            
    }

}