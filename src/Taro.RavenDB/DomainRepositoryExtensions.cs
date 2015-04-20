using Raven.Client.Indexes;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence.RavenDB
{
    public static class DomainRepositoryExtensions
    {
        public static IRavenQueryable<T> Query<T>(this IDomainRepository repository)
        {
            return ToRavenRepositoryWithAssert(repository).Session.Query<T>();
        }

        public static IRavenQueryable<T> Query<T, TIndexCreator>(this IDomainRepository repository)
            where TIndexCreator : AbstractIndexCreationTask, new()
        {
            return ToRavenRepositoryWithAssert(repository).Session.Query<T, TIndexCreator>();
        }

        public static IRavenQueryable<T> Query<T>(this IDomainRepository repository, string indexName, bool isMapReduce = false)
        {
            return ToRavenRepositoryWithAssert(repository).Session.Query<T>(indexName, isMapReduce);
        }

        static RavenDomainRepository ToRavenRepositoryWithAssert(IDomainRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            var ravenRepository = repository as RavenDomainRepository;
            if (ravenRepository == null)
                throw new ArgumentException("Requires " + typeof(RavenDomainRepository) + ".");

            return ravenRepository;
        }
    }
}
