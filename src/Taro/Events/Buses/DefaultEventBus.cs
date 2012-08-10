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
        private List<IEvent> _uncommittedEvents = new List<IEvent>();

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
                _uncommittedEvents.Add(evnt);
            }
        }

        public bool RegisterHandler(Type handlerType)
        {
            return _handlerRegistry.RegisterHandler(handlerType);
        }

        public void RegisterHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            _handlerRegistry.RegisterHandlers(assembliesToScan);
        }

        public bool UnregisterHandler(Type handlerType)
        {
            return _handlerRegistry.UnregisterHandler(handlerType);
        }

        public void UnregisterHandlers(Type eventType)
        {
            _handlerRegistry.UnregisterHandlers(eventType);
        }

        public void UnregisterAllHandlers()
        {
            _handlerRegistry.UnregisterAllHandlers();
        }
    }
}
