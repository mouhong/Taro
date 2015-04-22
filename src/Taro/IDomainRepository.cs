using System;

namespace Taro
{
    public interface IDomainRepository : IDisposable
    {
        void Save<T>(T aggregate) where T : AggregateRoot;
    }
}
