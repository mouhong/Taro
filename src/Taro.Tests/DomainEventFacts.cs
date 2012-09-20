using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using Taro.Tests.Events;
using Taro.Tests.Events.Buses;
using Taro.TestUtils.Events;
using Taro.Events.Buses;
using Taro.TestUtils.Events.Buses;
using Taro.Events;
using Taro.Data;
using Taro.TestUtils.Data;

namespace Taro.Tests
{
    public class DomainEventFacts
    {
        public class TheApplyMethod
        {
            [Fact]
            public void will_append_event_to_uncommitted_event_stream_if_immediate_event_handlers_succeeded()
            {
                TaroEnvironment.Instance.ImmediateEventBus = new MockEventBus();
                TaroEnvironment.Instance.PostCommitEventBus = new MockEventBus();

                DomainEvent.ClearAllThreadStaticPendingEvents();

                using (var scope = new UnitOfWorkScope(new MockUnitOfWork()))
                {
                    DomainEvent.Apply(new SomeEvent());

                    Assert.Equal(1, DomainEvent.GetThreadStaticPendingEvents().Count);
                    Assert.IsType(typeof(SomeEvent), DomainEvent.GetThreadStaticPendingEvents().First());
                }
            }

            [Fact]
            public void will_not_append_event_to_uncommitted_event_stream_if_immediate_event_handler_fails()
            {
                TaroEnvironment.Instance.ImmediateEventBus = new MockEventBus();
                TaroEnvironment.Instance.PostCommitEventBus = new MockEventBus();

                DomainEvent.ClearAllThreadStaticPendingEvents();

                var invoked = false;
                Action<IEvent> callback = evnt => { invoked = true; };

                TaroEnvironment.Instance.ImmediateEventBus.RegisterHandler(typeof(Handler1));

                try
                {
                    DomainEvent.Apply(new SomeEvent());
                }
                catch { }

                Assert.False(invoked);
            }

            public class Handler1 : AbstractImmediatelyEventHandler<SomeEvent>
            {
                public override void Handle(SomeEvent evnt)
                {
                    throw new NotSupportedException();
                }
            }
        }
    }
}
