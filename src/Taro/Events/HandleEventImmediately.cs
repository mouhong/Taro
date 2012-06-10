using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class HandleEventImmediately<TEvent> : IHandleEventImmediately<TEvent>
        where TEvent : IEvent
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected HandleEventImmediately()
            : this(ThreadStaticUnitOfWorkContext.Current)
        {
        }

        protected HandleEventImmediately(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract void Handle(TEvent evnt);
    }
}
