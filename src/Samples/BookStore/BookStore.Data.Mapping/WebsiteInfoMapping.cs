using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using BookStore.Domain;

namespace BookStore.Data.Mapping
{
    public class WebsiteInfoMapping : ClassMapping<WebsiteInfo>
    {
        public WebsiteInfoMapping()
        {
            Id(c => c.Key, m =>
            {
                m.Column("`Key`");
                m.Generator(Generators.Assigned);
            });

            Property(c => c.Value);
        }
    }
}
