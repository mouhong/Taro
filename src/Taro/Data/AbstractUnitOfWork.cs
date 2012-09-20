using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using Taro.Events;
using Taro.Events.Buses;
using Taro.Utils;

namespace Taro.Data
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private TransactionScopeOption _transactionScopeOption = TransactionScopeOption.Required;

        protected IEventBus EventBus { get; private set; }

        public TransactionScopeOption TransactionScopeOption
        {
            get
            {
                return _transactionScopeOption;
            }
            set
            {
                _transactionScopeOption = value;
            }
        }

        public bool DisableDTC { get; set; }

        protected AbstractUnitOfWork()
            : this(TaroEnvironment.Instance.PostCommitEventBus)
        {
        }

        protected AbstractUnitOfWork(IEventBus postCommitEventBus)
        {
            Require.NotNull(postCommitEventBus, "postCommitEventBus");
            EventBus = postCommitEventBus;
        }

        ~AbstractUnitOfWork()
        {
            Dispose(false);
        }

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            if (DisableDTC)
            {
                DoCommit();
                OnCommitted();
            }
            else
            {
                using (var scope = new TransactionScope(_transactionScopeOption))
                {
                    DoCommit();
                    OnCommitted();
                    scope.Complete();
                }
            }

            PostCommitActions.Clear();
            DomainEvent.ClearAllThreadStaticPendingEvents();
        }

        protected virtual void OnCommitted()
        {
            ExecutePostCommitActions();
            PublishPostCommitEvents();
        }

        protected virtual void ExecutePostCommitActions()
        {
            foreach (var action in PostCommitActions.GetQueuedActions())
            {
                action();
            }
        }

        protected virtual void PublishPostCommitEvents()
        {
            foreach (var evnt in DomainEvent.GetThreadStaticPendingEvents())
            {
                EventBus.Publish(evnt);
            }
        }

        protected abstract void DoCommit();

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
