using System;

namespace Taro.Dispatching
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public UnitOfWorkScope UnitOfWorkScope { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, UnitOfWorkScope unitOfWorkScope)
        {
            Phase = phase;
            UnitOfWorkScope = unitOfWorkScope; // can be null
        }
    }
}
