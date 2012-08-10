using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Taro.Events.Buses;
using Taro.Events;

namespace Taro.Tests.Events.Buses
{
    public class ImmediateEventHandlerRegistryFacts
    {
        public class TheFindHandlersMethod
        {
            [Fact]
            public void will_only_find_handlers_for_passed_in_event_type()
            {
                var finder = new ImmediateEventHandlerRegistry();
                finder.RegisterHandler(typeof(Event1Handler1));
                finder.RegisterHandler(typeof(Event1Handler2));
                finder.RegisterHandler(typeof(Event2Handler1));

                var handlers = finder.FindHandlers(typeof(Event1));
                var handlersList = new List<object>();

                foreach (var each in handlers)
                {
                    handlersList.Add(each);
                }

                Assert.Equal(2, handlersList.Count);
                Assert.IsType<Event1Handler1>(handlersList[0]);
                Assert.IsType<Event1Handler2>(handlersList[1]);
            }
        }

        public class TheRegisterHandlerMethod
        {
            [Fact]
            public void will_not_register_if_passed_handlerType_is_not_valid()
            {
                var finder = new ImmediateEventHandlerRegistry();

                Assert.False(finder.RegisterHandler(typeof(TheRegisterHandlerMethod)));
                Assert.False(finder.RegisterHandler(typeof(Event1)));

                Assert.Empty(finder.FindHandlers(typeof(Event1)));
            }

            [Fact]
            public void will_register_if_passed_handlerType_is_valid()
            {
                var finder = new ImmediateEventHandlerRegistry();
                Assert.True(finder.RegisterHandler(typeof(Event1Handler1)));

                var handlers = finder.FindHandlers(typeof(Event1));
                Assert.NotEmpty(handlers);

                var enumerator = handlers.GetEnumerator();
                enumerator.MoveNext();

                Assert.IsType<Event1Handler1>(enumerator.Current);
            }
        }

        public class Event1 : Event
        {
        }

        public class Event2 : Event
        {
        }

        public class Event1Handler1 : IImmediatelyEventHandler<Event1>
        {
            public void Handle(Event1 evnt)
            {
            }
        }

        public class Event1Handler2 : IImmediatelyEventHandler<Event1>
        {
            public void Handle(Event1 evnt)
            {
            }
        }

        public class Event2Handler1 : IImmediatelyEventHandler<Event2>
        {
            public void Handle(Event2 evnt)
            {
            }
        }
    }
}
