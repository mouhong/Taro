using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Taro.Events
{
    static class HandlerUtil
    {
        public static IEnumerable<Type> GetHandledEventTypes(Type handlerType)
        {
            foreach (var contract in handlerType.GetInterfaces())
            {
                if (!contract.IsGenericType) continue;

                if (contract.GetGenericTypeDefinition() == typeof(IHandle<>))
                {
                    var eventType = contract.GetGenericArguments().FirstOrDefault();
                    yield return eventType;
                }
            }
        }

        public static bool IsAttributeDefined(MethodInfo method, Type attributeType)
        {
            return method.IsDefined(attributeType, false) || method.DeclaringType.IsDefined(attributeType, false);
        }
    }
}
