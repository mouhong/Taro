using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class MessageMapping : ClassMapping<Message>
    {
        public MessageMapping()
        {
            Id(c => c.Id, m => m.Generator(Generators.Assigned));

            Property(c => c.Content);
            Property(c => c.SentTime);
            Property(c => c.UserId);
            Property(c => c.IsRead);
        }
    }
}
