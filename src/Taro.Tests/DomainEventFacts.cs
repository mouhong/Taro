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

namespace Taro.Tests
{
    public class DomainEventFacts
    {
        public class TheApplyMethod
        {
            [Fact]
            public void will_invoke_callbacks_on_immediate_event_success()
            {
                TaroEnvironment.Instance.ImmediateHandlerRegistry.Clear();
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                var invoked1 = false;
                Action<IEvent> callback1 = evnt => { invoked1 = true; };

                var invoked2 = false;
                Action<IEvent> callback2 = evnt => { invoked2 = true; };

                DomainEvent.RegisterThreadStaticEventAppliedCallback(callback1);
                DomainEvent.RegisterThreadStaticEventAppliedCallback(callback2);

                DomainEvent.Apply(new SomeEvent());

                Assert.True(invoked1);
                Assert.True(invoked2);
            }

            [Fact]
            public void will_not_invoke_callback_on_immediate_event_handler_failed()
            {
                TaroEnvironment.Instance.ImmediateHandlerRegistry.Clear();
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                var invoked = false;
                Action<IEvent> callback = evnt => { invoked = true; };

                DomainEvent.RegisterThreadStaticEventAppliedCallback(callback);

                TaroEnvironment.Instance.ImmediateHandlerRegistry.RegisterHandler(typeof(Handler1));

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

        public class TheRegisterThreadStaticEventAppliedCallbackMethod
        {
            [Fact]
            public void will_register_callback_to_current_thread_only()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                Action<IEvent> mainThreadCallback = evnt => { };

                DomainEvent.RegisterThreadStaticEventAppliedCallback(mainThreadCallback);

                Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());

                var childThread = new Thread(new ThreadStart(() =>
                {
                    Action<IEvent> childThreadCallback = evnt => { };

                    DomainEvent.RegisterThreadStaticEventAppliedCallback(mainThreadCallback);

                    Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());
                }));

                childThread.Start();
                childThread.Join();

                Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());
            }
        }

        public class TheUnregisterThreadStaticEventAppliedCallbackMethod
        {
            [Fact]
            public void will_unregister_current_thread_callbacks_only()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                Action<IEvent> mainThreadCallback = evnt => { };

                DomainEvent.RegisterThreadStaticEventAppliedCallback(mainThreadCallback);

                var childThread = new Thread(new ThreadStart(() =>
                {
                    var result = DomainEvent.UnregisterThreadStaticEventAppliedCallback(mainThreadCallback);
                    Assert.False(result, "Should not unregister other thread's callbacks");
                }));

                childThread.Start();
                childThread.Join();

                Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());

                DomainEvent.UnregisterThreadStaticEventAppliedCallback(mainThreadCallback);

                Assert.Equal(0, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());
            }
        }
    }
}
