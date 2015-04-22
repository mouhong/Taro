using Raven.Client;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using System;
using Taro.Transports;
using Taro.Workers;

namespace Taro.RavenDB
{
    public class RavenDomainRepository : DomainRepositoryBase, IRavenDomainRepository
    {
        public new RavenDomainDbSession Session
        {
            get { return (RavenDomainDbSession)base.Session; }
        }

        public RavenDomainRepository(IDocumentSession session, IRelayWorker relayWorker)
            : base(new RavenDomainDbSession(session), relayWorker)
        {
        }

        public T Find<T>(string id) where T : AggregateRoot
        {
            return Session.Session.Load<T>(id);
        }

        public T[] Find<T>(params string[] ids) where T : AggregateRoot
        {
            return Session.Session.Load<T>(ids);
        }

        public T[] Find<T>(params ValueType[] ids) where T : AggregateRoot
        {
            return Session.Session.Load<T>(ids);
        }

        public IRavenQueryable<T> Query<T>() where T : AggregateRoot
        {
            return Session.Session.Query<T>();
        }

        public IRavenQueryable<T> Query<T, TIndexCreator>()
            where T : AggregateRoot
            where TIndexCreator : AbstractIndexCreationTask, new()
        {
            return Session.Session.Query<T, TIndexCreator>();
        }

        public IRavenQueryable<T> Query<T>(string indexName, bool isMapReduce = false) where T : AggregateRoot
        {
            return Session.Session.Query<T>(indexName, isMapReduce);
        }

        public void Delete<T>(T aggregate) where T : AggregateRoot
        {
            Session.Session.Delete(aggregate);
            Session.Commit();
        }

        public void Delete(string id)
        {
            Session.Session.Delete(id);
            Session.Commit();
        }
    }
}
