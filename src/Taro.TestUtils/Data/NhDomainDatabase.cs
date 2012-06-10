using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace Taro.TestUtils.Data
{
    public static class NhDomainDatabase
    {
        public static ISessionFactory SessionFactory { get; set; }

        public static void Initialize(Configuration config)
        {
            SessionFactory = config.BuildSessionFactory();
        }

        public static void InitializeWithMssql(IEnumerable<Type> mappingTypes)
        {
            InitializeWithMssql(mappingTypes, "NhDomainDatabase");
        }

        public static void InitializeWithMssql(IEnumerable<Type> mappingTypes, string connectionStringName)
        {
            var config = new Configuration();
            config.DataBaseIntegration(x =>
            {
                x.ConnectionStringName = connectionStringName;
                x.Dialect<NHibernate.Dialect.MsSql2005Dialect>();
                x.Driver<NHibernate.Driver.SqlClientDriver>();
            });

            var modelMapper = new ModelMapper();
            modelMapper.AddMappings(mappingTypes);

            config.AddMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities());

            SessionFactory = config.BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }

        public static int ExecuteUpdate(string sql)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.CreateSQLQuery(sql).ExecuteUpdate();
            }
        }

        public static T Get<T>(object id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public static void Save(object entity)
        {
            using (var session = SessionFactory.OpenSession())
            using (var tran = session.BeginTransaction())
            {
                session.Save(entity);
                tran.Commit();
            }
        }
    }
}
