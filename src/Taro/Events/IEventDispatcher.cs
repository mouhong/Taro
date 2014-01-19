using System;

namespace Taro.Events
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent evnt, EventDispatchingContext context);
    }
}
