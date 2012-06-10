using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Buses
{
    static class EventHandlerFinderUtil
    {
        public static Type TryFindEventTypeOfImplementedHandlerInterface(Type type, Type handlerInterfaceOpenGenericType)
        {
            foreach (var inter in type.GetInterfaces())
            {
                if (inter.IsGenericType)
                {
                    var def = inter.GetGenericTypeDefinition();
                    var eventType = inter.GetGenericArguments().FirstOrDefault();

                    if (def == handlerInterfaceOpenGenericType)
                    {
                        return eventType;
                    }
                }
            }

            return null;
        }
    }
}
