using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public abstract class UnitOfWorkBase : IUnitOfWork
    {
        private bool isDisposed;

        public event EventHandler Comitted;

        ~UnitOfWorkBase()
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
            if (!isDisposed)
            {
                Dispose(true);
                GC.SuppressFinalize(this);
                isDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
