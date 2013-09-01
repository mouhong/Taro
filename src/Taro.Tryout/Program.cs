using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taro.Config;
using Taro.Events;
using Taro.Tryout.Data;
using Taro.Tryout.Domain;

namespace Taro.Tryout
{
    class Program
    {
        static void Main(string[] args)
        {
            // Global Configuration
            AppBootstrap();

            using (var scope = new UnitOfWorkScope())
            {
                Console.WriteLine("[Database] Begin transaction");
                Console.WriteLine();

                var customer = new Customer
                {
                    Name = "Mouhong"
                };

                // 1. Create a new order
                var order = new Order(customer);

                order.Items.Add(new OrderItem
                {
                    ProductName = "Vitamin C (550mg)",
                    Quantity = 1,
                    UnitPrice = 59
                });

                // 2. Mark order as payed
                order.AcceptPayment("Alipay");

                // 3. Deliver this order
                order.Deliver();

                // 4. Complete this order
                order.Complete();

                scope.Complete();

                Console.WriteLine();
                Console.WriteLine("[Database] End transaction");
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("[App] Main procedure exited");
            Console.WriteLine();

            Console.ReadKey();
        }

        static void AppBootstrap()
        {
            TaroEnvironment.Configure(taro =>
            {
                taro.UsingUnitOfWorkFactory(() => new InMemoryUnitOfWork());
                taro.UsingDefaultEventDispatcher(typeof(Program).Assembly);
            });
        }
    }
}
