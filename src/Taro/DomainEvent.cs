using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using Taro.Events;
using Taro.Events.Buses;
using System.Threading;
using Taro.Utils;

namespace Taro
{
    public static class DomainEvent
    {
        private static ThreadLocal<List<Action<IEvent>>> _eventAppliedCallbacks = new ThreadLocal<List<Action<IEvent>>>(() => new List<Action<IEvent>>());

        public static void Apply<TEvent>(TEvent evnt)
            where TEvent : IEvent
        {
            Require.NotNull(evnt, "evnt");

            foreach (var handler in TaroEnvironment.Instance.ImmediateHandlerRegistry.GetHandlers(evnt))
            {
                EventHandlerInvoker.Invoke(handler, evnt);
            }

            foreach (var callback in _eventAppliedCallbacks.Value)
            {
                callback(evnt);
            }
        }

        public static void RegisterThreadStaticEventAppliedCallback(Action<IEvent> callback)
        {
            Require.NotNull(callback, "callback");
            _eventAppliedCallbacks.Value.Add(callback);
        }

        public static bool UnregisterThreadStaticEventAppliedCallback(Action<IEvent> callback)
        {
            Require.NotNull(callback, "callback");
            return _eventAppliedCallbacks.Value.Remove(callback);
        }

        public static int GetRegisteredThreadStaticEventAppliedCallbackCount()
        {
            return _eventAppliedCallbacks.Value.Count;
        }

        internal static void ClearRegisteredThreadStaticEventAppliedCallbacks()
        {
            _eventAppliedCallbacks.Value.Clear();
        }
    }
}
