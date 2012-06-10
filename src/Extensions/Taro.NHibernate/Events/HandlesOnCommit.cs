using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class HandlesOnCommit<TEvent> : HandleEventOnCommit<TEvent>
        where TEvent : IEvent
    {
        protected new UnitOfWork UnitOfWork
        {
            get
            {
                return (UnitOfWork)base.UnitOfWork;
            }
        }

        protected HandlesOnCommit()
        {
        }
    }
}
