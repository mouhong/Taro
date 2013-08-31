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

        static ThreadLocal<List<IDomainEvent>> _uncommittedEvents = new ThreadLocal<List<IDomainEvent>>(() => new List<IDomainEvent>());

        public static IEnumerable<IDomainEvent> UncommittedEvents
        {
            get
            {
                return _uncommittedEvents.Value;
            }
        }

        public static void Apply<TEvent>(TEvent evnt)
            where TEvent : IDomainEvent
        {
            Require.NotNull(evnt, "evnt");

            var unitOfWork = UnitOfWorkAmbient.Current;

            if (unitOfWork == null)
                throw new InvalidOperationException("Domain event can only be applied within a unit of work scope.");

            var dispatcher = TaroEnvironment.Instance.EventDispatcher;

            if (dispatcher == null)
                throw new InvalidOperationException("Domain event dispatcher is not registered.");

            _uncommittedEvents.Value.Add(evnt);

            dispatcher.Dispatch(evnt, new EventDispathcingContext(unitOfWork, false));
        }

        public static void ClearUncommittedEvents()
        {
            _uncommittedEvents.Value.Clear();
        }
    }
}
