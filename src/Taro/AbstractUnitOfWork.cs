using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Taro.Config;
using Taro.Events;

namespace Taro
{
    public abstract class AbstractUnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        protected IEventDispatcher EventDispatcher { get; private set; }

        protected AbstractUnitOfWork()
            : this(TaroEnvironment.Instance.EventDispatcher)
        {
        }

        protected AbstractUnitOfWork(IEventDispatcher eventDispatcher)
        {
            Require.NotNull(eventDispatcher, "eventDispatcher");
            EventDispatcher = eventDispatcher;
        }

        ~AbstractUnitOfWork()
        {
            Dispose(false);
        }

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            DoCommit();
            OnCommitted();

            DomainEvent.ClearUncommittedEvents();
        }

        protected virtual void OnCommitted()
        {
            foreach (var evnt in DomainEvent.UncommittedEvents)
            {
                EventDispatcher.Dispatch(evnt, new EventDispathcingContext(this, true));
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
                    DomainEvent.ClearUncommittedEvents();
                }

                _disposed = true;
            }
        }
    }
}
