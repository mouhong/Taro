using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Taro.Transports.InProcess
{
    public class HandlerRegistry
    {
        private Dictionary<Type, List<HandlerMethod>> _handleMethods = new Dictionary<Type, List<HandlerMethod>>();

        public IEnumerable<HandlerMethod> GetHandlers(Type eventType)
        {
            List<HandlerMethod> handleMethods;
            if (_handleMethods.TryGetValue(eventType, out handleMethods))
            {
                return handleMethods;
            }

            return Enumerable.Empty<HandlerMethod>();
        }

        public void RegisterHandlers(IEnumerable<Type> handlerTypes)
        {
            foreach (var handlerType in handlerTypes)
            {
                foreach (var theInterface in handlerType.GetInterfaces())
                {
                    if (theInterface.IsGenericType && theInterface.GetGenericTypeDefinition() == typeof(IHandles<>))
                    {
                        var eventType = theInterface.GetGenericArguments()[0];
                        var handlerMethod = new HandlerMethod(handlerType.GetMethod("Handle", new[] { eventType }));

                        if (_handleMethods.ContainsKey(eventType))
                        {
                            _handleMethods[eventType].Add(handlerMethod);
                        }
                        else
                        {
                            _handleMethods.Add(eventType, new List<HandlerMethod> { handlerMethod });
                        }
                    }
                }
            }
        }

        public void RegisterHandlers(params Assembly[] assemblies)
        {
            RegisterHandlers(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterHandlers(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                RegisterHandlers(assembly.GetTypes());
            }
        }
    }
}
