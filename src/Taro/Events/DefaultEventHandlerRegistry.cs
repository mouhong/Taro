using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taro.Events
{
    public class DefaultEventHandlerRegistry : IEventHandlerRegistry
    {
        private ConcurrentDictionary<Type, ConcurrentBag<Type>> _eventHandlersTypeMap = new ConcurrentDictionary<Type, ConcurrentBag<Type>>();

        public IEnumerable<Type> FindHandlerTypes(Type eventType)
        {
            ConcurrentBag<Type> handlerTypes = null;

            if (_eventHandlersTypeMap.TryGetValue(eventType, out handlerTypes))
            {
                return handlerTypes;
            }

            return Enumerable.Empty<Type>();
        }

        public void RegisterHandlers(Assembly assembly)
        {
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
                var handlerTypes = _eventHandlersTypeMap.GetOrAdd(eventType, new ConcurrentBag<Type>());
                handlerTypes.Add(handlerType);
            }

            return true;
        }
    }
}
