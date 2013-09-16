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

        protected IList<IDomainEvent> UncommittedEvents { get; private set; }

        protected AbstractUnitOfWork()
            : this(TaroEnvironment.Instance.EventDispatcher)
        {
        }

        protected AbstractUnitOfWork(IEventDispatcher eventDispatcher)
        {
            Require.NotNull(eventDispatcher, "eventDispatcher");
            EventDispatcher = eventDispatcher;
            UncommittedEvents = new List<IDomainEvent>();
        }

        ~AbstractUnitOfWork()
        {
            Dispose(false);
        }

        public void ApplyEvent<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Require.NotNull(evnt, "evnt");
            UncommittedEvents.Add(evnt);
            EventDispatcher.Dispatch(evnt, new EventDispathcingContext(this, false));
        }

        public virtual void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot commit a disposed unit of work.");

            DoCommit();

            // Post commit event handlers might invoke Commit() on current unit of work again.
            // In this case, we must ensure that all the event handlers should not be invoked again,
            // because current unit of work is invoking them.
            // So we copy uncommitted events to a temporary list here, then clear it before invoking handlers.
            var events = UncommittedEvents.ToList();
            UncommittedEvents.Clear();

            if (events.Count > 0)
            {
                // If some handler invokes domain model during the execution of handlers, new events might araise.
                // In this case, if the handler doesn't commit the unit of work, we simply ignore the new events.
                // This events will be removed when the current unit of work is disposed or the current thread is exited.
                // If the handler commit the unit of work, because we already clear the events above,
                // so only handlers for the new events will be invoked.
                DispatchPostCommitEvents(events);
            }
        }

        private void DispatchPostCommitEvents(IEnumerable<IDomainEvent> events)
        {
            var context = new EventDispathcingContext(this, true);

            foreach (var evnt in events)
            {
                EventDispatcher.Dispatch(evnt, context);
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
                    UncommittedEvents.Clear();
                }

                _disposed = true;
            }
        }
    }
}
