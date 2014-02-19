using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Taro.Dispatching
{
    public class DefaultEventHandlerRegistry : IEventHandlerRegistry
    {
        private readonly object _writeLock = new object();
        private Dictionary<Type, List<MethodInfo>> _handlerMethodsByEventType = new Dictionary<Type, List<MethodInfo>>();

        public IEnumerable<MethodInfo> FindHandlerMethods(Type eventType)
        {
            Require.NotNull(eventType, "eventType");

            // Copy cache dictionary to a local variable to avoid concurrency issue.
            // No lock is required for reads because the writes are performed in copy-on-write mode
            var holder = _handlerMethodsByEventType;

            var handlerMethods = FindDirectHandlers(eventType, holder).ToList();

            // Here we need to support base event subscribtion:
            // If event A is raised, handlers subscribing to A and A's base events all need to be invoked.
            var baseEventType = eventType.BaseType;

            while (baseEventType != null && baseEventType != typeof(object))
            {
                handlerMethods.AddRange(FindDirectHandlers(baseEventType, holder));
                baseEventType = baseEventType.BaseType;
            }

            foreach (var implementedEventInterface in eventType.GetInterfaces())
            {
                if (typeof(IEvent).IsAssignableFrom(implementedEventInterface))
                {
                    handlerMethods.AddRange(FindDirectHandlers(implementedEventInterface, holder));
                }
            }

            return handlerMethods;
        }

        private IEnumerable<MethodInfo> FindDirectHandlers(Type eventType, Dictionary<Type, List<MethodInfo>> holder)
        {
            List<MethodInfo> handlerTypes = null;

            if (holder.TryGetValue(eventType, out handlerTypes))
            {
                return handlerTypes;
            }

            return Enumerable.Empty<MethodInfo>();
        }

        public void RegisterHandlers(IEnumerable<Type> handlerTypes)
        {
            lock (_writeLock)
            {
                // Copy on write
                var holder = CopyInnerDictionary();

                foreach (var type in handlerTypes)
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        RegisterHandler(type, holder);
                    }
                }

                // Replace current cache
                _handlerMethodsByEventType = holder;
            }
        }

        public void RegisterAssemblies(params Assembly[] assemblies)
        {
            Require.NotNull(assemblies, "assemblies");
            RegisterAssemblies(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterAssemblies(IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            var types = assemblies.SelectMany(x => x.GetTypes()).ToList();
            if (types.Count > 0)
            {
                RegisterHandlers(types);
            }
        }

        private bool RegisterHandler(Type handlerType, Dictionary<Type, List<MethodInfo>> holder)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract)
            {
                return false;
            }

            var eventTypes = TypeUtil.GetOpenGenericArgumentTypes(handlerType, typeof(IHandle<>)).ToList();

            if (eventTypes.Count == 0)
            {
                return false;
            }

            var thisHandlerMethods = handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                .Where(m => m.Name == "Handle" && m.ReturnType == typeof(void))
                                                .ToList();

            foreach (var eventType in eventTypes)
            {
                List<MethodInfo> handlerMethods = null;

                if (!holder.TryGetValue(eventType, out handlerMethods))
                {
                    handlerMethods = new List<MethodInfo>();
                    holder.Add(eventType, handlerMethods);
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

            return true;
        }

        public void Clear()
        {
            lock (_writeLock)
            {
                _handlerMethodsByEventType = new Dictionary<Type, List<MethodInfo>>();
            }
        }

        private Dictionary<Type, List<MethodInfo>> CopyInnerDictionary()
        {
            var dictionary = new Dictionary<Type, List<MethodInfo>>();
            foreach (var kvp in _handlerMethodsByEventType)
            {
                var methods = kvp.Value.ToList();
                dictionary.Add(kvp.Key, methods);
            }

            return dictionary;
        }
    }
}
