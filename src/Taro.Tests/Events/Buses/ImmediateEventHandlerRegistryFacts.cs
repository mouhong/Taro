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
            public void will_not_register_invalid_handler()
            {
                var registry = new ImmediateEventHandlerRegistry();

                Assert.False(registry.RegisterHandler(typeof(TheRegisterHandlerMethod)));
                Assert.False(registry.RegisterHandler(typeof(Event1)));

                Assert.Empty(registry.FindHandlers(typeof(Event1)));
            }

            [Fact]
            public void will_register_valid_handler()
            {
                var registry = new ImmediateEventHandlerRegistry();
                Assert.True(registry.RegisterHandler(typeof(Event1Handler1)));

                var handlers = registry.FindHandlers(typeof(Event1));
                Assert.NotEmpty(handlers);

                var enumerator = handlers.GetEnumerator();
                enumerator.MoveNext();

                Assert.IsType<Event1Handler1>(enumerator.Current);
            }

            [Fact]
            public void will_not_duplicate_register_existing_handler()
            {
                var registry = new ImmediateEventHandlerRegistry();
                registry.RegisterHandler(typeof(Event1Handler1));
                registry.RegisterHandler(typeof(Event1Handler1));

                var handlers = registry.FindHandlers(typeof(Event1));

                var enumerator = handlers.GetEnumerator();
                enumerator.MoveNext();

                Assert.IsType<Event1Handler1>(enumerator.Current);
                Assert.False(enumerator.MoveNext());
            }
        }

        public class TheUnregisterHandlerMethod
        {
            [Fact]
            public void can_unregister_handler()
            {
                var registry = new ImmediateEventHandlerRegistry();
                registry.RegisterHandler(typeof(Event1Handler1));
                registry.RegisterHandler(typeof(Event2Handler1));
                registry.RegisterHandler(typeof(Event2Handler2));

                Assert.True(registry.UnregisterHandler(typeof(Event2Handler1)));

                var enumerator = registry.FindHandlers(typeof(Event2)).GetEnumerator();
                Assert.True(enumerator.MoveNext());

                Assert.IsType<Event2Handler2>(enumerator.Current);
            }

            [Fact]
            public void will_return_false_for_non_existing_handler()
            {
                var registry = new ImmediateEventHandlerRegistry();
                registry.RegisterHandler(typeof(Event1Handler1));

                Assert.False(registry.UnregisterHandler(typeof(Event1Handler2)));
            }
        }

        public class TheUnregisterHandlersMethod
        {
            [Fact]
            public void can_unregister_handlers()
            {
                var registry = new ImmediateEventHandlerRegistry();
                registry.RegisterHandler(typeof(Event1Handler1));
                registry.RegisterHandler(typeof(Event2Handler1));
                registry.RegisterHandler(typeof(Event2Handler2));

                registry.UnregisterHandlers(typeof(Event2));

                var enumerator = registry.FindHandlers(typeof(Event2)).GetEnumerator();
                Assert.False(enumerator.MoveNext());

                enumerator = registry.FindHandlers(typeof(Event1)).GetEnumerator();
                Assert.True(enumerator.MoveNext());
                Assert.IsType<Event1Handler1>(enumerator.Current);
            }
        }

        public class TheUnregisterAllHandlersMethod
        {
            [Fact]
            public void can_unregister_all_handlers()
            {
                var register = new ImmediateEventHandlerRegistry();
                register.RegisterHandler(typeof(Event1Handler1));
                register.RegisterHandler(typeof(Event2Handler1));
                register.RegisterHandler(typeof(Event2Handler2));

                register.UnregisterAllHandlers();

                Assert.False(register.FindHandlers(typeof(Event1)).GetEnumerator().MoveNext());
                Assert.False(register.FindHandlers(typeof(Event2)).GetEnumerator().MoveNext());
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

        public class Event2Handler2 : IImmediatelyEventHandler<Event2>
        {
            public void Handle(Event2 evnt)
            {
            }
        }
    }
}
