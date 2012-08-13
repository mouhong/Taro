using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;

using Taro.Data;
using Taro.Events.Buses;
using Taro.Events.Storage;

namespace Taro.TestUtils.Data
{
    public class NhUnitOfWork : AbstractUnitOfWork
    {
        public ISession Session { get; set; }

        public NhUnitOfWork(ISession session, IEventBus eventBus, IEventStore eventStore)
            : base(eventBus, eventStore)
        {
            Session = session;
        }

        public T Get<T>(object id)
        {
            return Session.Get<T>(id);
        }

        public IQueryable<T> Query<T>()
        {
            return Session.Query<T>();
        }

        protected override void DoCommit()
        {
            using (var tran = Session.BeginTransaction())
            {
                tran.Commit();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Session != null)
            {
                Session.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
