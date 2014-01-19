using System;

namespace Taro.Events
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public EventContext UnitOfWorkScope { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, EventContext unitOfWorkScope)
        {
            Phase = phase;
            UnitOfWorkScope = unitOfWorkScope;
        }
    }
}
