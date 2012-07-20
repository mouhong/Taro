using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Cfg;

namespace BookStore.Data
{
    public class SessionManager
    {
        public bool IsInitialized { get; private set; }

        public Configuration Configuration { get; private set; }

        public ISessionFactory SessionFactory { get; private set; }

        public SessionManager()
        {
        }

        public void Initailize(Configuration config)
        {
            Configuration = config;
            SessionFactory = Configuration.BuildSessionFactory();

            IsInitialized = true;
        }

        public ISession OpenSession()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Not yet initialized.");

            return SessionFactory.OpenSession();
        }

        public static readonly SessionManager Current = new SessionManager();
    }
}
