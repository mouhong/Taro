using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Events
{
    public abstract class HandlesImmediately<TEvent> : HandleEventImmediately<TEvent>
        where TEvent : IEvent
    {
        protected new UnitOfWork UnitOfWork
        {
            get
            {
                return (UnitOfWork)base.UnitOfWork;
            }
        }

        protected HandlesImmediately()
        {
        }
    }
}
