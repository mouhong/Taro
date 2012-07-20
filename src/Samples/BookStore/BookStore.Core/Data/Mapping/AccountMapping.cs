using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace BookStore.Data.Mapping
{
    public class AccountMapping : ClassMapping<Account>
    {
        public AccountMapping()
        {
            Id(c => c.Id, m => m.Generator(Generators.Assigned));

            Property(c => c.Balance);

            Component<AccountNotificationSettings>(c => c.NotificationSettings, m =>
            {
                m.Property(x => x.Enabled, x => x.Column("NotificationSettings_Enabled"));
                m.Property(x => x.MinAmount, x => x.Column("NotificationSettings_MinAmount"));
            });

            Bag<AccountLog>(c => c.Logs, m =>
            {
                m.Key(k => k.Column("AccountId"));
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                m.Inverse(true);
            }, m => m.OneToMany());
        }
    }
}
