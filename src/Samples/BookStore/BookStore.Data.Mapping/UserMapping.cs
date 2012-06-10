using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

using BookStore.Domain;

namespace BookStore.Data.Mapping
{
    public class UserMapping : ClassMapping<User>
    {
        public UserMapping()
        {
            Table("`User`");

            Id(c => c.Id, m => m.Generator(Generators.Assigned));

            Property(c => c.NickName);
            Property(c => c.Email);
            Property(c => c.Password);
            Property(c => c.Gender);
            Property(c => c.CreatedTime);

            ManyToOne<Account>(c => c.Account, m => m.Column("AccountId"));

            Bag<BuyedBook>(c => c.BuyedBooks, m =>
            {
                m.Key(k => k.Column("UserId"));
                m.Cascade(Cascade.All | Cascade.DeleteOrphans);
                m.Inverse(true);
            }, m => m.OneToMany());
        }
    }
}
