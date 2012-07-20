using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;
using Taro.Events;
using Taro.Events.Buses;
using Taro.Data;

namespace BookStore.Data
{
    public class NhUnitOfWork : AbstractUnitOfWork
    {
        public ISession Session { get; private set; }

        public NhUnitOfWork(ISession session)
        {
            Session = session;
        }

        public NhUnitOfWork(ISession session, IEventBus eventBus)
            : base(eventBus)
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

        public IQueryOver<T, T> QueryOver<T>()
            where T : class
        {
            return Session.QueryOver<T>();
        }

        public void Save(object entity)
        {
            Session.Save(entity);
        }

        public void Delete(object entity)
        {
            Session.Delete(entity);
        }

        protected override void CommitChanges()
        {
            using (var tran = Session.BeginTransaction())
            {
                tran.Commit();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Session.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
