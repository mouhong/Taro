using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taro.Events
{
    public class DefaultEventHandlerRegistry : IEventHandlerRegistry
    {
        private Dictionary<Type, List<MethodInfo>> _handlerMethodsByEventType = new Dictionary<Type, List<MethodInfo>>();

        public IEnumerable<MethodInfo> FindHandlerMethods(Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            var handlerMethods = FindDirectHandlers(eventType).ToList();

            // Here we need to support base event subscribtion:
            // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
            var baseEventType = eventType.BaseType;

            while (baseEventType != null && typeof(IDomainEvent).IsAssignableFrom(baseEventType))
            {
                handlerMethods.AddRange(FindDirectHandlers(baseEventType));
                baseEventType = baseEventType.BaseType;
            }

            return handlerMethods;
        }

        private IEnumerable<MethodInfo> FindDirectHandlers(Type eventType)
        {
            List<MethodInfo> handlerTypes = null;

            if (_handlerMethodsByEventType.TryGetValue(eventType, out handlerTypes))
            {
                return handlerTypes;
            }

            return Enumerable.Empty<MethodInfo>();
        }

        public void RegisterHandlers(IEnumerable<Type> handlerTypes)
        {
            lock (_handlerMethodsByEventType)
            {
                foreach (var type in handlerTypes)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        RegisterHandler(type);
                    }
                }
            }
        }

        public void RegisterHandlers(Assembly assembly)
        {
            Require.NotNull(assembly, "assembly");
            RegisterHandlers(assembly.GetTypes());
        }

        public bool RegisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract)
            {
                return false;
            }

            var eventTypes = HandlerUtil.GetHandledEventTypes(handlerType).ToList();

            if (eventTypes.Count == 0)
            {
                return false;
            }

            lock (_handlerMethodsByEventType)
            {
                var thisHandlerMethods = handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                    .Where(m => m.Name == "Handle" && m.ReturnType == typeof(void))
                                                    .ToList();

                foreach (var eventType in eventTypes)
                {
                    List<MethodInfo> handlerMethods = null;

                    if (!_handlerMethodsByEventType.TryGetValue(eventType, out handlerMethods))
                    {
                        handlerMethods = new List<MethodInfo>();
                        _handlerMethodsByEventType.Add(eventType, handlerMethods);
                    }

                    foreach (var method in thisHandlerMethods)
                    {
                        var parameters = method.GetParameters();
                        if (parameters.Length == 1 && parameters[0].ParameterType == eventType)
                        {
                            handlerMethods.Add(method);
                            break;
                        }
                    }
                }
            }

            return true;
        }

        public bool RemoveHandlers(Type eventType)
        {
            lock (_handlerMethodsByEventType)
            {
                return _handlerMethodsByEventType.Remove(eventType);
            }
        }

        public void Clear()
        {
            lock (_handlerMethodsByEventType)
            {
                _handlerMethodsByEventType.Clear();
            }
        }
    }
}
