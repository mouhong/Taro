using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public interface IHandles<in TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent theEvent);
    }
}
