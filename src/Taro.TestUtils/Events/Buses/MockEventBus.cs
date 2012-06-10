using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events.Buses;

namespace Taro.TestUtils.Events.Buses
{
    public class MockEventBus : IEventBus
    {
        public Action<object> PublishAction { get; set; }

        public MockEventBus()
        {
        }

        public MockEventBus(Action<object> publishAction)
        {
            PublishAction = publishAction;
        }

        public void Publish<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            if (PublishAction != null)
            {
                PublishAction(evnt);
            }
        }
    }
}
