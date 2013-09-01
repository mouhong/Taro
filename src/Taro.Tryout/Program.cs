using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taro.Config;
using Taro.Events;

namespace Taro.Tryout
{
    class Program
    {
        static void Main(string[] args)
        {
            TaroEnvironment.Configure(x =>
            {
                x.UsingUnitOfWorkFactory(() => new UnitOfWork());
                x.UsingDefaultEventDispatcher(typeof(Program).Assembly);
            });

            using (var scope = new UnitOfWorkScope())
            {
                var order = new Order
                {
                    Id = 5515
                };

                order.AcceptPayment("Alipay");
                order.Deliver();

                scope.Complete();
            }

            Console.WriteLine();
            Console.WriteLine(">> Press any key to continue...");
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
