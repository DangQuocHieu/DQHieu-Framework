namespace DQHieu.Framework.DesignPattern
{
    using System;
    public static class EventBus<T> where T : IGameEvent
    {
        private static Action<T> _onEvent;

        public static void Subscribe(Action<T> listener)
        {
            _onEvent += listener;
        }

        public static void Unsubscribe(Action<T> listener)
        {
            _onEvent -= listener;
        }

        public static void Raise(T eventData)
        {
            _onEvent?.Invoke(eventData);
        }
            
    }

}