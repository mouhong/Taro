using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class MessageBoxInfoMapping : ClassMapping<MessageBoxInfo>
    {
        public MessageBoxInfoMapping()
        {
            Id(c => c.UserId, m => m.Generator(Generators.Assigned));

            Property(c => c.TotalMessages);
            Property(c => c.UnReadMessages);
        }
    }
}
