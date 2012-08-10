using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events.Buses;
using System.Reflection;

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

        public bool RegisterHandler(Type handlerType)
        {
            throw new NotImplementedException();
        }

        public void RegisterHandlers(IEnumerable<Assembly> assembliesToScan)
        {
            throw new NotImplementedException();
        }

        public bool UnregisterHandler(Type handlerType)
        {
            throw new NotImplementedException();
        }

        public void UnregisterHandlers(Type eventType)
        {
            throw new NotImplementedException();
        }

        public void UnregisterAllHandlers()
        {
            throw new NotImplementedException();
        }
    }
}
