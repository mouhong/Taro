using System;
using System.Collections;
using System.Collections.Generic;

namespace Taro.Events.Buses
{
    public interface IOnCommitEventHandlerFinder
    {
        IEnumerable FindHandlers(IEvent evnt);
    }
}
