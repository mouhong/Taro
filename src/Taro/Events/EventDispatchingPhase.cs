using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events
{
    public enum EventDispatchingPhase
    {
        OnEventRaised = 0,
        OnUnitOfWorkCommitted = 1
    }
}
