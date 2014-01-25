using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Taro.Dispatching
{
    static class TypeUtil
    {
        public static IEnumerable<Type> GetOpenGenericArgumentTypes(Type concreteType, Type openGenericType)
        {
            foreach (var @interface in concreteType.GetInterfaces())
            {
                if (!@interface.IsGenericType) continue;

                if (@interface.GetGenericTypeDefinition() == openGenericType)
                {
                    foreach (var argType in @interface.GetGenericArguments())
                    {
                        yield return argType;
                    }
                }
            }
        }

        public static bool IsAttributeDefinedInMethodOrDeclaringClass(MethodInfo method, Type attributeType)
        {
            return method.IsDefined(attributeType, false) || method.DeclaringType.IsDefined(attributeType, false);
        }
    }
}
