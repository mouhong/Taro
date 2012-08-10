using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Buses
{
    public class PostCommitEventHandlerRegistry : AbstractEventHandlerRegistry
    {
        protected override Type ExtractEventType(Type handlerType)
        {
            return EventHandlerFinderUtil.TryFindEventTypeOfImplementedHandlerInterface(handlerType, typeof(IPostCommitEventHandler<>));
        }
    }
}
