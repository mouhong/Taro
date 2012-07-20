using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class BuyedBookMapping : ClassMapping<BuyedBook>
    {
        public BuyedBookMapping()
        {
            Id(c => c.Id, m => m.Generator(Generators.Assigned));

            Property(c => c.Price);
            Property(c => c.BuyTime);

            ManyToOne<User>(c => c.Owner, m => m.Column("UserId"));
            ManyToOne<Book>(c => c.Book, m => m.Column("BookISBN"));
        }
    }
}
