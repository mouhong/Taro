using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Taro.Utils;

namespace Taro.Events.Buses
{
    public abstract class AbstractEventHandlerRegistry : IEventHandlerRegistry
    {
        // Key: Event Type, Value: Handler Type
        private Dictionary<Type, List<Type>> _handlerTypes = new Dictionary<Type, List<Type>>();
        private readonly object _lock = new object();

        public IEnumerable FindHandlers(Type eventType)
        {
            Require.NotNull(eventType, "eventType");

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

            lock (_lock)
            {
                return NonThreadSafeRegisterHandler(handlerType);
            }
        }

        private bool NonThreadSafeRegisterHandler(Type handlerType)
        {
            if (!handlerType.IsClass || handlerType.IsAbstract || handlerType.IsGenericType) return false;

            var eventType = ExtractEventType(handlerType);

            if (eventType != null)
            {
                if (!_handlerTypes.ContainsKey(eventType))
                {
                    _handlerTypes.Add(eventType, new List<Type>());
                }

                var handlerTypes = _handlerTypes[eventType];

                if (!handlerTypes.Contains(handlerType))
                {
                    handlerTypes.Add(handlerType);
                }

                return true;
            }

            return false;
        }

        public void RegisterHandlers(params System.Reflection.Assembly[] assembliesToScan)
        {
            Require.NotNull(assembliesToScan, "assembliesToScan");

            lock (_lock)
            {
                foreach (var assembly in assembliesToScan)
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        NonThreadSafeRegisterHandler(type);
                    }
                }
            }
        }

        public bool UnregisterHandler(Type handlerType)
        {
            lock (_lock)
            {
                foreach (var each in _handlerTypes)
                {
                    if (each.Value.Remove(handlerType))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void UnregisterHandlers(Type eventType)
        {
            if (!_handlerTypes.ContainsKey(eventType)) return;

            lock (_lock)
            {
                if (_handlerTypes.ContainsKey(eventType))
                {
                    _handlerTypes.Remove(eventType);
                }
            }
        }

        public void UnregisterAllHandlers()
        {
            lock (_lock)
            {
                _handlerTypes.Clear();
            }
        }

        protected abstract Type ExtractEventType(Type handlerType);
    }
}
