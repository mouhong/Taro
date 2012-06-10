using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events
{
    public interface IHandleEventOnCommit<in TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent evnt);
    }
}
