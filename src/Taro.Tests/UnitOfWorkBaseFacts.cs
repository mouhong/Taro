using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events;
using Taro.Tests.Events;
using Xunit;

namespace Taro.Tests
{
    public class UnitOfWorkBaseFacts
    {
        public class TheApplyEventMethod
        {
            [Fact]
            public void WillDispatchEventToDirectHandlersAndAddToUncommittedList()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { typeof(Event1Handler1) });

                using (var scope = new UnitOfWorkScope<MockUnitOfWork>(new MockUnitOfWork(dispatcher)))
                {
                    var evnt = new Event1();
                    scope.UnitOfWork.ApplyEvent(evnt);

                    Assert.Equal("Handled", evnt.Data);
                    Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                    Assert.Equal(1, scope.UnitOfWork.UncommittedEvents.Count);
                }
            }

            public class Event1 : DomainEvent
            {
                public string Data { get; set; }
            }

            public class Event1Handler1 : IHandle<Event1>
            {
                public void Handle(Event1 evnt)
                {
                    evnt.Data = "Handled";
                }
            }
        }

        public class TheCommitMethod
        {
            [Fact]
            public void WillClearUncommittedEventsAfterExecution()
            {
                var dispatcher = new DefaultEventDispatcher();
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { typeof(Event1Handler1) });

                using (var scope = new UnitOfWorkScope<MockUnitOfWork>(new MockUnitOfWork(dispatcher)))
                {
                    scope.UnitOfWork.ApplyEvent(new Event1());
                    Assert.Equal(1, scope.UnitOfWork.UncommittedEvents.Count);
                    scope.UnitOfWork.Commit();
                    Assert.Equal(0, scope.UnitOfWork.UncommittedEvents.Count);
                }
            }

            [Fact]
            public void CanDispatchEventsRaisedInPostCommitHandlers()
            {
                var handlerInvoker = new CountedHandlerInvoker();
                var dispatcher = new DefaultEventDispatcher(new DefaultEventHandlerRegistry(), handlerInvoker);
                dispatcher.HandlerRegistry.RegisterHandlers(new[] { 
                    typeof(Event1Handler1),
                    typeof(Event1PostCommitHandler1),
                    typeof(Event2Handler1)
                });

                using (var scope = new UnitOfWorkScope<MockUnitOfWork>(new MockUnitOfWork(dispatcher)))
                {
                    scope.UnitOfWork.ApplyEvent(new Event1());

                    Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                    Assert.Equal(0, handlerInvoker.GetTotalInvoked(typeof(Event2Handler1)));

                    scope.UnitOfWork.Commit();

                    Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event1Handler1)));
                    Assert.Equal(1, handlerInvoker.GetTotalInvoked(typeof(Event2Handler1)));
                }
            }

            public class Event1 : DomainEvent
            {
            }

            public class Event2 : DomainEvent
            {
            }

            public class Event1Handler1 : IHandle<Event1>
            {
                public void Handle(Event1 evnt)
                {
                }
            }

            [AwaitCommitted]
            public class Event1PostCommitHandler1 : IHandle<Event1>
            {
                public void Handle(Event1 evnt)
                {
                    var uow = (MockUnitOfWork)UnitOfWorkAmbient.Current;
                    uow.ApplyEvent(new Event2());
                }
            }

            public class Event2Handler1 : IHandle<Event2>
            {
                public void Handle(Event2 evnt)
                {
                }
            }
        }
    }
}
