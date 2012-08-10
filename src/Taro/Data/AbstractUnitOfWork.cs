using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Taro.Events;
using Taro.Events.Buses;
using Taro.Events.Storage;
using Taro.Utils;

namespace Taro.Data
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private TransactionScopeOption _transactionScopeOption = TransactionScopeOption.Required;

        protected IEventBus EventBus { get; private set; }

        protected IEventStore EventStore { get; private set; }

        public TransactionScopeOption TransactionScopeOption
        {
            get
            {
                return _transactionScopeOption;
            }
        }

        protected AbstractUnitOfWork()
            : this(TaroEnvironment.Instance.EventBus, null)
        {
        }

        protected AbstractUnitOfWork(IEventBus eventBus)
            : this(eventBus, null)
        {
        }

        protected AbstractUnitOfWork(IEventBus eventBus, IEventStore eventStore)
        {
            Require.NotNull(eventBus, "eventBus");

            EventBus = eventBus;
            EventStore = eventStore;
        }

        ~AbstractUnitOfWork()
        {
            Dispose(false);
        }

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            using (var scope = new TransactionScope(_transactionScopeOption))
            {
                CommitChanges();

                foreach (var action in PostCommitActions.GetQueuedActions())
                {
                    action();
                }

                var pendingEvents = DomainEvent.GetThreadStaticPendingEvents();

                if (EventStore != null)
                {
                    EventStore.SaveEvents(pendingEvents);
                }

                foreach (var evnt in pendingEvents)
                {
                    EventBus.Publish(evnt);
                }

                scope.Complete();
            }

            PostCommitActions.Clear();
            DomainEvent.ClearAllThreadStaticPendingEvents();
        }

        protected abstract void CommitChanges();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    PostCommitActions.Clear();
                    DomainEvent.ClearAllThreadStaticPendingEvents();
                }

                _disposed = true;
            }
        }
    }
}
