using System;
using System.Collections;

namespace Taro.Events.Buses
{
    public interface IImmediateEventHandlerFinder
    {
        IEnumerable FindHandlers(IEvent evnt);
    }
}
