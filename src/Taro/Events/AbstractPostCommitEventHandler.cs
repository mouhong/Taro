using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class AbstractPostCommitEventHandler<TEvent> : IPostCommitEventHandler<TEvent>
        where TEvent : IEvent
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected AbstractPostCommitEventHandler()
            : this(ThreadStaticUnitOfWorkContext.Current)
        {
        }

        protected AbstractPostCommitEventHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract void Handle(TEvent evnt);
    }
}
