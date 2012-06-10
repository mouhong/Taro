using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class HandleEventOnCommit<TEvent> : IHandleEventOnCommit<TEvent>
        where TEvent : IEvent
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected HandleEventOnCommit()
            : this(ThreadStaticUnitOfWorkContext.Current)
        {
        }

        protected HandleEventOnCommit(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract void Handle(TEvent evnt);
    }
}
