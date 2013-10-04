using System;
using System.Collections.Generic;
using System.Threading;
using Taro.Config;
using Taro.Events;

namespace Taro
{
    [Serializable]
    public abstract class DomainEvent : IDomainEvent
    {
        public DateTime UtcTimestamp { get; private set; }

        protected DomainEvent()
            : this(DateTime.UtcNow)
        {
        }

        protected DomainEvent(DateTime utcTimestamp)
        {
            UtcTimestamp = utcTimestamp;
        }

        public static void Apply<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Require.NotNull(evnt, "evnt");

            var unitOfWork = (UnitOfWorkBase)UnitOfWorkAmbient.Current;

            if (unitOfWork == null)
                throw new InvalidOperationException("Domain event can only be applied within a unit of work scope.");

            unitOfWork.ApplyEvent(evnt);
        }
    }
}
