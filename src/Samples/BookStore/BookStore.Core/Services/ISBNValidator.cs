using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;

namespace BookStore.Services
{
    public class ISBNValidator
    {
        private ISession _session;

        public ISBNValidator(ISession session)
        {
            _session = session;
        }

        public bool ExistsISBN(string isbn)
        {
            return _session.Query<Book>().Any(it => it.ISBN == isbn);
        }
    }
}
