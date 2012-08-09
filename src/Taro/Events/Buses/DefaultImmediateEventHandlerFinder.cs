using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;
using Taro.Utils;

namespace Taro.Events.Buses
{
    public class DefaultImmediateEventHandlerFinder : IImmediateEventHandlerFinder
    {
        // Key: Event Type, Value: Handler Type
        private Dictionary<Type, List<Type>> _handlerTypes = new Dictionary<Type, List<Type>>();

        public DefaultImmediateEventHandlerFinder()
        {
        }

        public IEnumerable FindHandlers(IEvent evnt)
        {
            Require.NotNull(evnt, "evnt");

            var eventType = evnt.GetType();

            if (_handlerTypes.ContainsKey(eventType))
            {
                foreach (var handlerType in _handlerTypes[eventType])
                {
                    yield return Activator.CreateInstance(handlerType);
                }
            }
        }

        public bool RegisterHandler(Type handlerType)
        {
            Require.NotNull(handlerType, "handlerType");

            if (!handlerType.IsClass || handlerType.IsAbstract || handlerType.IsGenericType) return false;

            var eventType = EventHandlerFinderUtil.TryFindEventTypeOfImplementedHandlerInterface(handlerType, typeof(IImmediatelyEventHandler<>));

            if (eventType != null)
            {
                if (!_handlerTypes.ContainsKey(eventType))
                {
                    _handlerTypes.Add(eventType, new List<Type>());
                }
                _handlerTypes[eventType].Add(handlerType);

                return true;
            }

            return false;
        }

        public void RegisterHandlers(params Assembly[] assemblies)
        {
            RegisterHandlers(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterHandlers(IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    RegisterHandler(type);
                }
            }
        }

        public void Clear()
        {
            _handlerTypes.Clear();
        }
    }
}
