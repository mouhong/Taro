using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events;
using Xunit;

namespace Taro.Tests.Events
{
    public class DefaultEventDispatcherFacts
    {
        public class TheDispatchMethod
        {
            [Fact]
            public void WillNotInvokeAwaitCommittedHandlersInPhase_OnEventRaised()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { typeof(Event1Handler1), typeof(Event1Handler2), typeof(Event1AwaitCommittedHandler1) });

                var uow = new MockUnitOfWork();

                using (var scope = UnitOfWorkScope.Begin(uow, dispatcher))
                {
                    var context = new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, scope);
                    dispatcher.Dispatch(new Event1(), context);
                }

                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler2)));
                Assert.Equal(0, handlerInvoker.GetTotalInvoked(typeof(Event1AwaitCommittedHandler1)));
            }

            [Fact]
            public void WillNotInvokeDirectHandlersInPhase_OnUnitOfWorkCommitted()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { typeof(Event1Handler1), typeof(Event1Handler2), typeof(Event1AwaitCommittedHandler1) });

                var uow = new MockUnitOfWork();

                using (var scope = UnitOfWorkScope.Begin(uow, dispatcher))
                {
                    var context = new EventDispatchingContext(EventDispatchingPhase.OnUnitOfWorkCommitted, scope);
                    dispatcher.Dispatch(new Event1(), context);
                }

                Assert.Equal(0, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                Assert.Equal(0, handlerInvoker.GetTotalInvoked(typeof(Event1Handler2)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1AwaitCommittedHandler1)));
            }

            [Fact]
            public void WillInvokeAllHandlersIfNotWithinUnitOfWorkScope()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { typeof(Event1Handler1), typeof(Event1Handler2), typeof(Event1AwaitCommittedHandler1) });

                var uow = new MockUnitOfWork();

                var context = new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, null);
                dispatcher.Dispatch(new Event1(), context);

                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler2)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1AwaitCommittedHandler1)));
            }

            [Fact]
            public void SupportBaseEventSubscription()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { 
                    typeof(Event1Handler1), 
                    typeof(Event1Handler2), 
                    typeof(DerivedEvent1Handler1), 
                    typeof(DerivedDerivedEvent1Handler1) });

                var uow = new MockUnitOfWork();

                var context = new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, null);

                // 1 level
                dispatcher.Dispatch(new DerivedEvent1(), context);

                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler2)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(DerivedEvent1Handler1)));

                handlerInvoker.ClearCounter();

                // 2 level
                dispatcher.Dispatch(new DerivedDerivedEvent1(), context);

                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler2)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(DerivedEvent1Handler1)));
                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(DerivedDerivedEvent1Handler1)));
            }

            [Fact]
            public void OneHandlerCanSubscribeToMultiEvents()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { 
                    typeof(Event1And2Handler1)
                });

                var uow = new MockUnitOfWork();
                var context = new EventDispatchingContext(EventDispatchingPhase.OnEventRaised, null);

                var event1 = new Event1();
                dispatcher.Dispatch(event1, context);

                Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1And2Handler1)));
                Assert.Equal("Event1 Handled", event1.Data);

                var event2 = new Event2();
                dispatcher.Dispatch(event2, context);

                Assert.Equal(2, handlerInvoker.GetTotalInvoked(typeof(Event1And2Handler1)));
                Assert.Equal("Event2 Handled", event2.Data);
            }

            #region Events & Handlers

            public class Event1 : DomainEvent
            {
                public string Data { get; set; }
            }

            public class Event2 : DomainEvent
            {
                public string Data { get; set; }
            }

            public class DerivedEvent1 : Event1 { }

            public class DerivedDerivedEvent1 : DerivedEvent1 { }

            public class Event1Handler1 : IHandle<Event1>
            {
                public void Handle(Event1 evnt) { }
            }

            public class Event1Handler2 : IHandle<Event1>
            {
                public void Handle(Event1 evnt) { }
            }

            public class DerivedEvent1Handler1 : IHandle<DerivedEvent1>
            {
                public void Handle(DerivedEvent1 evnt) { }
            }

            public class DerivedDerivedEvent1Handler1 : IHandle<DerivedDerivedEvent1>
            {
                public void Handle(DerivedDerivedEvent1 evnt) { }
            }

            [AwaitCommitted]
            public class Event1AwaitCommittedHandler1 : IHandle<Event1>
            {
                public void Handle(Event1 evnt) { }
            }

            public class Event1And2Handler1 : IHandle<Event1>, IHandle<Event2>
            {
                public void Handle(Event1 evnt)
                {
                    evnt.Data = "Event1 Handled";
                }

                public void Handle(Event2 evnt)
                {
                    evnt.Data = "Event2 Handled";
                }
            }

            #endregion
        }
    }
}
