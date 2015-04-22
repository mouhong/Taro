using Raven.Client.Indexes;
using Raven.Client.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro
{
    public interface IRavenDomainRepository : IDomainRepository
    {
        T Find<T>(string id) where T : AggregateRoot;

        T[] Find<T>(params string[] ids) where T : AggregateRoot;

        T[] Find<T>(params ValueType[] ids) where T : AggregateRoot;

        IRavenQueryable<T> Query<T>() where T : AggregateRoot;

        IRavenQueryable<T> Query<T, TIndexCreator>() 
            where T : AggregateRoot
            where TIndexCreator : AbstractIndexCreationTask, new();

        IRavenQueryable<T> Query<T>(string indexName, bool isMapReduce = false) where T : AggregateRoot;

        void Delete<T>(T aggregate) where T : AggregateRoot;

        void Delete(string id);
    }
}
