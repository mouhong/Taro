using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Dispatching;

namespace Taro
{
    public abstract class CommitableBase : ICommitable
    {
        private bool _isDisposed;

        public event EventHandler Comitted;

        protected IEventDispatcher EventDispatcher { get; private set; }

        protected CommitableBase(IEventDispatcher eventDispatcher)
        {
            Require.NotNull(eventDispatcher, "eventDispatcher");
            EventDispatcher = eventDispatcher;
            UnitOfWorkScope.Begin(this, EventDispatcher);
        }

        ~CommitableBase()
        {
            Dispose(false);
        }

        public virtual void Commit()
        {
            DoCommit();
            OnComitted();
        }

        protected abstract void DoCommit();

        protected virtual void OnComitted()
        {
            if (Comitted != null)
                Comitted(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                _isDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            UnitOfWorkScope.Current.Dispose();
        }
    }
}
