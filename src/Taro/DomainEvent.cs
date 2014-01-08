using System;
using System.Collections.Generic;
using System.Threading;
using Taro.Config;
using Taro.Context;
using Taro.Events;

namespace Taro
{
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime UtcTimestamp { get; private set; }

        protected DomainEvent()
            : this(DateTime.UtcNow)
        {
        }

        protected DomainEvent(DateTime utcTimestamp)
        {
            UtcTimestamp = utcTimestamp;
        }

        static ThreadLocal<List<Action<IDomainEvent>>> _eventAppliedCallbacks = new ThreadLocal<List<Action<IDomainEvent>>>(() => new List<Action<IDomainEvent>>());

        public static void Apply<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Require.NotNull(evnt, "evnt");

            var dispatcher = Taro.Config.TaroEnvironment.Instance.EventDispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Cannot resolve event dispatcher. Ensure event dispatcher is registered.");

            dispatcher.Dispatch(evnt, new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, UnitOfWorkScopeContext.Current));

            OnEventApplied(evnt);
        }

        static void OnEventApplied(IDomainEvent evnt)
        {
            var callbacks = _eventAppliedCallbacks.Value;
            foreach (var callback in callbacks)
            {
                callback(evnt);
            }
        }

        public static void RegisterEventAppliedCallback(Action<IDomainEvent> callback)
        {
            Require.NotNull(callback, "callback");
            _eventAppliedCallbacks.Value.Add(callback);
        }

        public static void UnregisterEventAppliedCallback(Action<IDomainEvent> callback)
        {
            Require.NotNull(callback, "callback");
            _eventAppliedCallbacks.Value.Remove(callback);
        }
    }
}
