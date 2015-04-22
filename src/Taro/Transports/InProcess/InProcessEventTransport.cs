using System;
using System.Collections.Generic;

namespace Taro.Transports.InProcess
{
    public class InProcessEventTransport : IEventTransport
    {
        private IHandlerActivator _handlerActivator;

        public IHandlerActivator HandlerActivator
        {
            get
            {
                if (_handlerActivator == null)
                {
                    _handlerActivator = new DefaultHandlerActivator();
                }
                return _handlerActivator;
            }
            set
            {
                _handlerActivator = value;
            }
        }
        
        public HandlerRegistry Registry { get; private set; }

        public InProcessEventTransport()
        {
            Registry = new HandlerRegistry();
        }

        public void Send(IEvent @event)
        {
            var eventType = @event.GetType();
            var eventTypes = new List<Type>{ eventType };

            var baseEventType = eventType.BaseType;

            while (baseEventType != null && typeof(IEvent).IsAssignableFrom(baseEventType))
            {
                eventTypes.Add(baseEventType);
                baseEventType = baseEventType.BaseType;
            }

            foreach (var @interface in eventType.GetInterfaces())
            {
                if (typeof(IEvent).IsAssignableFrom(@interface))
                {
                    eventTypes.Add(@interface);
                }
            }

            foreach (var type in eventTypes)
            {
                var handlers = FindDirectHandlers(type);
                InvokeHandlers(@event, handlers);
            }
        }

        private IEnumerable<HandlerMethod> FindDirectHandlers(Type eventType)
        {
            return Registry.GetHandlers(eventType);
        }

        private void InvokeHandlers(IEvent @event, IEnumerable<HandlerMethod> handlers)
        {
            foreach (var method in handlers)
            {
                object handler;

                try
                {
                    handler = HandlerActivator.CreateInstance(method.ReflectedType);
                }
                catch (Exception ex)
                {
                    throw new EventHandlerException("Fail to activate event handler: " + method.ReflectedType + ". See inner exception for details.", ex);
                }

                try
                {
                    method.Invoke(handler, @event);
                }
                catch (Exception ex)
                {
                    throw new EventHandlerException("Fail to execute event handler: " + method.ReflectedType + ". See inner exception for details.", ex);
                }
            }
        }
    }
}
