using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taro.Events
{
    public class DefaultEventHandlerRegistry : IEventHandlerRegistry
    {
        private ConcurrentDictionary<Type, ConcurrentBag<Type>> _handlerTypesByEventType = new ConcurrentDictionary<Type, ConcurrentBag<Type>>();

        public IEnumerable<Type> FindHandlerTypes(Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            var handlers = FindDirectHandlers(eventType).ToList();

            // Here we need to support base event subscribtion:
            // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
            var baseEventType = eventType.BaseType;

            while (baseEventType != null && typeof(IDomainEvent).IsAssignableFrom(baseEventType))
            {
                handlers.AddRange(FindDirectHandlers(baseEventType));
                baseEventType = baseEventType.BaseType;
            }

            return handlers;
        }

        private IEnumerable<Type> FindDirectHandlers(Type eventType)
        {
            ConcurrentBag<Type> handlerTypes = null;

            if (_handlerTypesByEventType.TryGetValue(eventType, out handlerTypes))
            {
                return handlerTypes;
            }

            return Enumerable.Empty<Type>();
        }

        public void RegisterHandlers(Assembly assembly)
        {
            Require.NotNull(assembly, "assembly");

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    RegisterHandler(type);
                }
            }
        }

        public bool RegisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract)
            {
                return false;
            }

            var eventTypes = TypeUtil.GetHandledEventTypes(handlerType).ToList();

            if (eventTypes.Count == 0)
            {
                return false;
            }

            foreach (var eventType in eventTypes)
            {
                var handlerTypes = _handlerTypesByEventType.GetOrAdd(eventType, new ConcurrentBag<Type>());
                handlerTypes.Add(handlerType);
            }

            return true;
        }
    }
}
