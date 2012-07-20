using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class AccountLogMapping : ClassMapping<AccountLog>
    {
        public AccountLogMapping()
        {
            Id(c => c.Id, m => m.Generator(Generators.Assigned));

            Property(c => c.AccountId);
            Property(c => c.Message);
            Property(c => c.Amount);
            Property(c => c.LogTime);
        }
    }
}
