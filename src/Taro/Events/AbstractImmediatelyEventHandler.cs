using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class AbstractImmediatelyEventHandler<TEvent> : IImmediatelyEventHandler<TEvent>
        where TEvent : IEvent
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected AbstractImmediatelyEventHandler()
            : this(ThreadStaticUnitOfWorkContext.Current)
        {
        }

        protected AbstractImmediatelyEventHandler(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract void Handle(TEvent evnt);
    }
}
