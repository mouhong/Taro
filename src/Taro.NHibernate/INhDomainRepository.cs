using System.Linq;

namespace Taro
{
    public interface INhDomainRepository : IDomainRepository
    {
        T Find<T>(object id) where T : AggregateRoot;

        IQueryable<T> Query<T>() where T : AggregateRoot;

        void Delete<T>(T aggregate) where T : AggregateRoot;
    }
}
