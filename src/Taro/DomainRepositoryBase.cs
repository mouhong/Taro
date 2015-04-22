using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Notification;
using Taro.Persistence;
using Taro.Transports;

namespace Taro
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        protected IDomainDbSession Session { get; private set; }

        protected INewEventNotifier NewEventNotifier { get; private set; }

        protected DomainRepositoryBase(IDomainDbSession session, INewEventNotifier newEventNotifier)
        {
            Require.NotNull(session, "session");
            Require.NotNull(newEventNotifier, "newEventNotifier");

            Session = session;
            NewEventNotifier = newEventNotifier;
        }

        public virtual void Save<T>(T aggregate) where T : AggregateRoot
        {
            Require.NotNull(aggregate, "aggregate");

            Session.SaveAggregate(aggregate);

            var events = ((IEventSource)aggregate).Events;
            Session.AddEvents(events);

            Session.Commit();

            NewEventNotifier.Notify();
        }

        public virtual void Dispose()
        {
            Session.Dispose();
        }
    }
}
