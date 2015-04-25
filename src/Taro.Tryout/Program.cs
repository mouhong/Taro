using Raven.Client;
using Raven.Client.Document;
using System;
using System.Linq;
using Taro.RavenDB.Indexes;
using Taro.Transports.InProcess;
using Taro.Tryout.Domain;

namespace Taro.Tryout
{
    public static class Database
    {
        public static readonly IDocumentStore Store;

        static Database()
        {
            Store = new DocumentStore
            {
                Url = "http://localhost:8080",
                DefaultDatabase = "Taro"
            };

            Store.Initialize();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AppRuntime.Instance
                      .Configure(cfg =>
                      {
                          cfg.UseRavenDB(Database.Store);
                          cfg.RunRelayWorkerInCurrentProcess();
                      })
                      .Start();

            CreateCustomer();
            ApproveCustomer("customers/1");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void AppStart()
        {
            Database.Store.ExecuteIndex(new StoredEventIndex());

            Database.Store.Conventions.JsonContractResolver = new Taro.RavenDB.Serialization.AggregateRootContractResolver();

            var transport = new InProcessEventTransport();
            transport.Registry.RegisterHandlers(new[] { typeof(Program).Assembly });
        }

        static void CreateCustomer()
        {
            var customer = new Customer
            {
                Id = "customers/1",
                FullName = "Mouhong",
                Email = "mouhong@gmail.com"
            };

            using (var repo = AppRuntime.Instance.CreateDomainRepository<IRavenDomainRepository>())
            {
                repo.Save(customer);
            }
        }

        static void ApproveCustomer(string customerId)
        {
            using (var repo = AppRuntime.Instance.CreateDomainRepository<IRavenDomainRepository>())
            {
                var customer = repo.Find<Customer>(customerId);
                customer.Approve();

                repo.Save(customer);
            }
        }
    }
}
