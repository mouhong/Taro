using System;

namespace Taro.Dispatching
{
    public interface IEventDispatcher
    {
        void Dispatch(IEvent evnt, EventDispatchingContext context);
    }
}
