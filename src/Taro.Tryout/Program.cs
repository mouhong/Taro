using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;
using Taro.Persistence.RavenDB;
using Taro.RavenDB.Persistence;
using Taro.Transports;
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

    public static class AppConfig
    {
        public static IRelayWorker RelayWorker;

        public static IEventTransport EventTransport;

        public static IRavenDomainRepository CreateRepository()
        {
            return new RavenDomainRepository(Database.Store.OpenSession(), RelayWorker);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AppStart();

            CreateCustomer();
            ApproveCustomer("customers/1");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void AppStart()
        {
            var transport = new InProcessEventTransport();
            transport.Registry.RegisterHandlers(new[] { typeof(Program).Assembly });

            AppConfig.EventTransport = transport;
            AppConfig.RelayWorker = new RelayWorker(() => new RavenDomainDbSession(Database.Store.OpenSession()), AppConfig.EventTransport);

            AppConfig.RelayWorker.Start();
        }

        static void CreateCustomer()
        {
            var customer = new Customer
            {
                Id = "customers/1",
                FullName = "Mouhong",
                Email = "mouhong@gmail.com"
            };

            using (var repo = AppConfig.CreateRepository())
            {
                repo.Save(customer);
            }
        }

        static void ApproveCustomer(string customerId)
        {
            using (var repo = AppConfig.CreateRepository())
            {
                var customer = repo.Find<Customer>(customerId);
                customer.Approve();

                repo.Save(customer);
            }
        }
    }
}
