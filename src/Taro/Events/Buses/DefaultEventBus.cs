using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Taro.Events.Storage;

namespace Taro.Events.Buses
{
    public class DefaultEventBus : IEventBus
    {
        private IPostCommitEventHandlerFinder _handlerFinder;
        private List<IEvent> _uncommittedEvents = new List<IEvent>();

        public DefaultEventBus(IPostCommitEventHandlerFinder handlerFinder)
        {
            _handlerFinder = handlerFinder;
        }

        public void Publish<TEvent>(TEvent evnt) where TEvent : IEvent
        {
            foreach (var handler in _handlerFinder.FindHandlers(evnt))
            {
                EventHandlerInvoker.Invoke(handler, evnt);
                _uncommittedEvents.Add(evnt);
            }
        }
    }
}
