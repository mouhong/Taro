using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Taro.Utils;

namespace Taro.Events.Buses
{
    public class DefaultEventBus : IEventBus
    {
        private IEventHandlerRegistry _handlerRegistry;

        public DefaultEventBus(IEventHandlerRegistry eventHandlerRegistry)
        {
            Require.NotNull(eventHandlerRegistry, "eventHandlerRegistry");
            _handlerRegistry = eventHandlerRegistry;
        }

        public void Publish<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            Require.NotNull(evnt, "evnt");

            var eventType = evnt.GetType();

            foreach (var handler in _handlerRegistry.FindHandlers(eventType))
            {
                EventHandlerInvoker.Invoke(handler, evnt);
            }
        }

        public bool RegisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");
            return _handlerRegistry.RegisterHandler(handlerType);
        }

        public void RegisterHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            Require.NotNull(assembliesToScan, "assembliesToScan");
            _handlerRegistry.RegisterHandlers(assembliesToScan);
        }

        public bool UnregisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");
            return _handlerRegistry.UnregisterHandler(handlerType);
        }

        public void UnregisterHandlers(Type eventType)
        {
            Require.NotNull(eventType, "eventType");
            _handlerRegistry.UnregisterHandlers(eventType);
        }

        public void UnregisterAllHandlers()
        {
            _handlerRegistry.UnregisterAllHandlers();
        }
    }
}
