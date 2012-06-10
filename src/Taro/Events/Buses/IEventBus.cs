using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Buses
{
    public interface IEventBus
    {
        void Publish<TEvent>(TEvent evnt) where TEvent : IEvent;
    }
}
