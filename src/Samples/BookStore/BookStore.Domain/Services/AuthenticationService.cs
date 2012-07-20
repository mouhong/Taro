using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;

namespace BookStore.Domain.Services
{
    public class AuthenticationService
    {
        private ISession _session;

        public AuthenticationService(ISession session)
        {
            _session = session;
        }

        public bool Authenticate(string email, string password)
        {
            return _session.Query<User>()
                           .Any(it => it.Email == email && it.Password == password);
        }
    }
}
