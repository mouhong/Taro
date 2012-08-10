using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Utils;
using System.Reflection;

namespace Taro.Events.Buses
{
    public class ImmediateEventHandlerRegistry : AbstractEventHandlerRegistry
    {
        protected override Type ExtractEventType(Type handlerType)
        {
            return EventHandlerFinderUtil.TryFindEventTypeOfImplementedHandlerInterface(handlerType, typeof(IImmediatelyEventHandler<>));
        }
    }
}
