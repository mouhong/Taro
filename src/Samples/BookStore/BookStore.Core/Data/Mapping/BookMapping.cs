using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class BookMapping : ClassMapping<Book>
    {
        public BookMapping()
        {
            Id(c => c.ISBN, m => m.Generator(Generators.Assigned));

            Property(c => c.Title);
            Property(c => c.Author);
            Property(c => c.Price);
            Property(c => c.Stock);
            Property(c => c.PublishedDate);
            Property(c => c.CreatorId);
        }
    }
}
