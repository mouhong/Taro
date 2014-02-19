using System;
using System.Collections.Generic;
using System.Threading;
using Taro.Config;
using Taro.Dispatching;

namespace Taro
{
    public static class Event
    {
        static ThreadLocal<List<Action<IEvent>>> _eventAppliedCallbacks = new ThreadLocal<List<Action<IEvent>>>(() => new List<Action<IEvent>>());

        public static void Apply<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            Require.NotNull(@event, "event");

            var dispatcher = Taro.Config.TaroEnvironment.Instance.EventDispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Cannot resolve event dispatcher. Ensure event dispatcher is registered.");

            dispatcher.Dispatch(@event, new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, UnitOfWorkScope.Current));

            OnEventApplied(@event);
        }

        static void OnEventApplied(IEvent evnt)
        {
            var callbacks = _eventAppliedCallbacks.Value;
            foreach (var callback in callbacks)
            {
                callback(evnt);
            }
        }

        public static void RegisterEventAppliedCallback(Action<IEvent> callback)
        {
            Require.NotNull(callback, "callback");
            _eventAppliedCallbacks.Value.Add(callback);
        }

        public static void UnregisterEventAppliedCallback(Action<IEvent> callback)
        {
            Require.NotNull(callback, "callback");
            _eventAppliedCallbacks.Value.Remove(callback);
        }
    }
}
