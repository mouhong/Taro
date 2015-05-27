using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;
using Taro.Transports;
using Taro.Workers;

namespace Taro
{
    public abstract class DomainRepositoryBase : IDomainRepository
    {
        protected IDomainDbSession Session { get; private set; }

        protected IRelayWorker RelayWorker { get; private set; }

        protected DomainRepositoryBase(IDomainDbSession session, IRelayWorker relayWorker)
        {
            Require.NotNull(session, "session");
            Require.NotNull(relayWorker, "relayWorker");

            Session = session;
            RelayWorker = relayWorker;
        }

        public virtual void Save<T>(T aggregate) where T : AggregateRoot
        {
            Require.NotNull(aggregate, "aggregate");

            Session.SaveAggregate(aggregate);

            var events = ((IEventSource)aggregate).Events;
            Session.AddEvents(events);

            Session.Commit();

            RelayWorker.Signal();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Session.Dispose();
            }
        }
    }
}
