using Raven.Client;
using System;
using System.Linq;

namespace Taro.Persistence.RavenDB
{
    public class RavenDomainRepository : DomainRepositoryBase
    {
        private IDocumentSession _session;

        public RavenDomainRepository(IDocumentSession session, IEventBus eventBus)
            : base(eventBus)
        {
            _session = session;
        }

        public override T Find<T>(object id)
        {
            if (id is String)
            {
                return _session.Load<T>((String)id);
            }
            if (id is ValueType)
            {
                return _session.Load<T>((ValueType)id);
            }

            throw new NotSupportedException("Only string or value type id is supported.");
        }

        protected override void SaveWithoutCommit<T>(T obj)
        {
            _session.Store(obj);
        }

        protected override void DeleteWithoutCommit<T>(T obj)
        {
            _session.Delete(obj);
        }

        protected override ILocalTransactionContext GetLocalTransactionContext()
        {
            return new RavenLocalTransactionContext(_session, false);
        }
    }
}
