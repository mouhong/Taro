using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;

namespace Taro
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        protected IEventBus EventBus { get; private set; }

        protected DomainRepositoryBase(IEventBus eventBus)
        {
            EventBus = eventBus;
        }

        public abstract T Find<T>(object id) where T : AggregateRoot;

        public virtual void Save<T>(T aggregate) where T : AggregateRoot
        {
            SaveWithoutCommit(aggregate);

            var localTransactionContext = GetCurrentLocalTransactionContext();
            foreach (var evnt in ((IEventSource)aggregate).Events)
            {
                EventBus.Publish(evnt, localTransactionContext);
            }

            Commit();
        }

        public virtual void Delete<T>(T aggregate) where T : AggregateRoot
        {
            DeleteWithoutCommit(aggregate);
            Commit();
        }

        protected abstract void SaveWithoutCommit<T>(T aggregate) where T : AggregateRoot;

        protected abstract void DeleteWithoutCommit<T>(T aggregate) where T : AggregateRoot;

        protected abstract ILocalTransactionContext GetCurrentLocalTransactionContext();

        protected abstract void Commit();
    }
}
