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
        private Action<IEvent> _eventAppliedCallback;
        private TransactionScopeOption _transactionScopeOption = TransactionScopeOption.Required;

        protected IEventBus EventBus { get; private set; }

        protected IEventStore EventStore { get; private set; }

        public UncommittedEventStream UncommittedEvents { get; private set; }

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
            UncommittedEvents = new UncommittedEventStream();

            _eventAppliedCallback = new Action<IEvent>(OnEventApplied);

            DomainEvent.RegisterThreadStaticEventAppliedCallback(_eventAppliedCallback);
        }

        ~AbstractUnitOfWork()
        {
            Dispose(false);
        }

        protected virtual void OnEventApplied(IEvent evnt)
        {
            UncommittedEvents.Append(evnt);
        }

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            if (UncommittedEvents.Count == 0 && PostCommitActions.Count() == 0)
            {
                CommitChanges();
            }
            else
            {
                using (var scope = new TransactionScope(_transactionScopeOption))
                {
                    CommitChanges();

                    if (EventStore != null)
                    {
                        EventStore.SaveEvents(UncommittedEvents);
                    }

                    foreach (var evnt in UncommittedEvents)
                    {
                        EventBus.Publish(evnt);
                    }

                    foreach (var action in PostCommitActions.GetQueuedActions())
                    {
                        action();
                    }

                    scope.Complete();
                }
            }

            UncommittedEvents.Clear();
            PostCommitActions.Clear();
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
                    DomainEvent.UnregisterThreadStaticEventAppliedCallback(_eventAppliedCallback);
                }

                _disposed = true;
            }
        }
    }
}
