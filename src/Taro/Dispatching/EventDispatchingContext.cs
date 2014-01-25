using System;

namespace Taro.Dispatching
{
    public class EventDispatchingContext
    {
        public EventDispatchingPhase Phase { get; private set; }

        public UnitOfWorkScope EventAmbient { get; private set; }

        public EventDispatchingContext(EventDispatchingPhase phase, UnitOfWorkScope eventAmbient)
        {
            Phase = phase;
            EventAmbient = eventAmbient;
        }
    }
}
