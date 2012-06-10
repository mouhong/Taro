using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Taro.Tests.Domain.Mapping
{
    public class AMap : ClassMapping<A>
    {
        public AMap()
        {
            Table(typeof(A).Name);

            Id(c => c.Id, m =>
            {
                m.Generator(Generators.Assigned);
            });

            Property(c => c.Value);
        }
    }
}
