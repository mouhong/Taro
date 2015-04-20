using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence.NHibernate;
using NHibernate.Linq;

namespace Taro
{
    public static class DomainRepositoryExtensions
    {
        public static IQueryable<T> Query<T>(this IDomainRepository repository)
        {
            var nhRepository = repository as NhDomainRepository;
            if (nhRepository == null)
                throw new InvalidOperationException("Requires " + typeof(NhDomainRepository) + ".");

            return nhRepository.Session.Query<T>();
        }
    }
}
