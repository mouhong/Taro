using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;
using Taro.Transports;

namespace Taro
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        protected IEventBus EventBus { get; private set; }

        protected IRelayWorker RelayWorker { get; private set; }

        protected DomainRepositoryBase(IEventBus eventBus, IRelayWorker relayWorker)
        {
            Require.NotNull(eventBus, "eventBus");
            Require.NotNull(relayWorker, "relayWorker");

            EventBus = eventBus;
            RelayWorker = relayWorker;
        }

        public abstract T Find<T>(object id) where T : AggregateRoot;

        public virtual void Save<T>(T aggregate) where T : AggregateRoot
        {
            Require.NotNull(aggregate, "aggregate");

            using (var context = CreateLocalTransactionContext())
            {
                SaveWithoutCommit(aggregate);

                var localTransactionContext = CreateLocalTransactionContext();
                foreach (var evnt in ((IEventSource)aggregate).Events)
                {
                    EventBus.Publish(evnt, localTransactionContext);
                }

                context.Commit();
            }

            RelayWorker.Signal();
        }

        public virtual void Delete<T>(T aggregate) where T : AggregateRoot
        {
            Require.NotNull(aggregate, "aggregate");

            using (var context = CreateLocalTransactionContext())
            {
                DeleteWithoutCommit(aggregate);
                context.Commit();
            }
        }

        protected abstract void SaveWithoutCommit<T>(T aggregate) where T : AggregateRoot;

        protected abstract void DeleteWithoutCommit<T>(T aggregate) where T : AggregateRoot;

        protected abstract ILocalTransactionContext CreateLocalTransactionContext();

        public abstract void Dispose();
    }
}
