using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using Taro.Data;
using Taro.Tests.Events;
using Taro.Tests.Events.Buses;
using Taro.TestUtils.Data;
using Taro.TestUtils.Events;
using Taro.TestUtils.Events.Storage;
using Taro.TestUtils.Events.Buses;
using Taro.Tests.Domain;
using Taro.Tests.Domain.Mapping;
using Taro.Events.Buses;
using Taro.Events.Storage;

namespace Taro.Tests.Data
{
    public class AbstractUnitOfWorkFacts
    {
        [Fact]
        public void will_append_uncommitted_event_on_event_applied()
        {
            var unitOfWork = new MockUnitOfWork();

            DomainEvent.Apply(new SomeEvent());

            Assert.Equal(1, unitOfWork.UncommittedEvents.Count);
        }

        public class TheConstructor
        {
            [Fact]
            public void will_register_event_applied_callback()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                var unitOfWork = new MockUnitOfWork();
                Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());
            }
        }

        public class TheCommitMethod
        {
            [Fact]
            public void will_clear_uncommitted_events_after_commit()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                var unitOfWork = new MockUnitOfWork();

                DomainEvent.Apply(new SomeEvent());

                unitOfWork.Commit();

                Assert.Equal(0, ((AbstractUnitOfWork)unitOfWork).UncommittedEvents.Count);
            }

            [Fact]
            public void will_raise_exception_if_is_disposed()
            {
                var unitOfWork = new MockUnitOfWork();
                unitOfWork.Dispose();

                Assert.Throws<ObjectDisposedException>(new Assert.ThrowsDelegate(() =>
                {
                    unitOfWork.Commit();
                }));
            }

            [Fact]
            public void will_rollback_domain_database_if_post_commit_event_publishing_fails()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                TaroEnvironment.Instance.EventBus = new MockEventBus(evnt =>
                {
                    throw new InvalidOperationException("Failed publishing post commit event.");
                });

                NhDomainDatabase.InitializeWithMssql(new[] { typeof(AMap) });

                NhDomainDatabase.ExecuteUpdate("DELETE FROM A");
                NhDomainDatabase.Save(new A { Id = 1, Value = 0 });

                var unitOfWork = new NhUnitOfWork(NhDomainDatabase.OpenSession(), TaroEnvironment.Instance.EventBus, new NullEventStore());

                try
                {
                    var a = unitOfWork.Get<A>(1);
                    a.AddValue(3);
                    unitOfWork.Commit();
                }
                catch { }

                unitOfWork.Dispose();

                Assert.Equal(0, NhDomainDatabase.Get<A>(1).Value);
            }

            [Fact]
            public void will_rollback_domain_database_if_event_store_fails()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                TaroEnvironment.Instance.EventBus = new MockEventBus();
                
                NhDomainDatabase.InitializeWithMssql(new[] { typeof(AMap) });

                NhDomainDatabase.ExecuteUpdate("DELETE FROM A");
                NhDomainDatabase.Save(new A { Id = 1, Value = 0 });

                var eventStore = new MockEventStore(events => { throw new InvalidOperationException("Failed saving events."); });
                var unitOfWork = new NhUnitOfWork(NhDomainDatabase.OpenSession(), TaroEnvironment.Instance.EventBus, eventStore);

                try
                {
                    var a = unitOfWork.Get<A>(1);
                    a.AddValue(3);
                    unitOfWork.Commit();
                }
                catch { }

                unitOfWork.Dispose();

                Assert.Equal(0, NhDomainDatabase.Get<A>(1).Value);
            }

            [Fact]
            public void will_commit_domain_model_change_if_all_not_fail()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                TaroEnvironment.Instance.EventBus = new MockEventBus();

                NhDomainDatabase.InitializeWithMssql(new[] { typeof(AMap) });

                NhDomainDatabase.ExecuteUpdate("DELETE FROM A");
                NhDomainDatabase.Save(new A { Id = 1, Value = 0 });

                var eventStore = new MockEventStore();
                var unitOfWork = new NhUnitOfWork(NhDomainDatabase.OpenSession(), TaroEnvironment.Instance.EventBus, eventStore);

                var a = unitOfWork.Session.Get<A>(1);
                a.AddValue(3);
                unitOfWork.Commit();

                unitOfWork.Dispose();

                Assert.Equal(3, NhDomainDatabase.Get<A>(1).Value);
            }

        }

        public class TheDisposeMethod
        {
            [Fact]
            public void will_unregister_event_applied_callback()
            {
                DomainEvent.ClearRegisteredThreadStaticEventAppliedCallbacks();

                var unitOfWork = new MockUnitOfWork();

                Assert.Equal(1, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());

                unitOfWork.Dispose();

                Assert.Equal(0, DomainEvent.GetRegisteredThreadStaticEventAppliedCallbackCount());
            }
        }
    }
}
