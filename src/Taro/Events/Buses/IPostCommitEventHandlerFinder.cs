using System;
using System.Collections;
using System.Collections.Generic;

namespace Taro.Events.Buses
{
    public interface IPostCommitEventHandlerFinder
    {
        IEnumerable FindHandlers(IEvent evnt);
    }
}
