using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using BookStore.Data;

namespace Taro.Events
{
    public abstract class ImmediatelyEventHandler<TEvent> : AbstractImmediatelyEventHandler<TEvent>
        where TEvent : IEvent
    {
        protected new NhUnitOfWork UnitOfWork
        {
            get
            {
                return (NhUnitOfWork)base.UnitOfWork;
            }
        }

        protected ImmediatelyEventHandler()
        {
        }
    }
}
