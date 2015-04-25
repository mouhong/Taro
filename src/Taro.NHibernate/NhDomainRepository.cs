using NHibernate;
using NHibernate.Linq;
using System.Linq;
using Taro.Persistence;
using Taro.Transports;
using Taro.Workers;

namespace Taro.NHibernate
{
    public class NhDomainRepository : DomainRepositoryBase, INhDomainRepository
    {
        public new NhDomainDbSession Session
        {
            get { return (NhDomainDbSession)base.Session; }
        }

        // Note: Pass in IDomainDbSession instead of NhDomainDbSession to make it IoC container friendly
        public NhDomainRepository(IDomainDbSession session, IRelayWorker relayWorker)
            : base(session, relayWorker)
        {
        }

        public T Find<T>(object id) where T : AggregateRoot
        {
            return Session.Session.Get<T>(id);
        }

        public IQueryable<T> Query<T>() where T : AggregateRoot
        {
            return Session.Session.Query<T>();
        }

        public void Delete<T>(T aggregate) where T : AggregateRoot
        {
            Session.Session.Delete(aggregate);
            Session.Commit();
        }
    }
}
