using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using System.Configuration;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

using Taro;
using Taro.Data;
using Taro.Events;
using BookStore.Data;

namespace BookStore.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            Initialize();
        }

        private void Initialize()
        {
            // Setup NHibernate

            var config = new NHibernate.Cfg.Configuration();
            config.DataBaseIntegration(cfg =>
            {
                cfg.Driver<NHibernate.Driver.Sql2008ClientDriver>();
                cfg.Dialect<NHibernate.Dialect.MsSql2008Dialect>();
                cfg.ConnectionStringName = "ConnectionString";
            });

            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.Load("BookStore.Core").GetTypes());

            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            SessionManager.Current.Initailize(config);

            TaroEnvironment.Configure(env =>
            {
                env.RegisterEventHandlers(Assembly.Load("BookStore.Core"))
                   .RegisterUnitOfWorkFactory(() => new NhUnitOfWork(SessionManager.Current.OpenSession()));
            });
        }
    }
}