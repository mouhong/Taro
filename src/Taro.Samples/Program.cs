using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Config;
using Taro.Samples.Data;
using Taro.Samples.Domain;

namespace Taro.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            AppBootstrap();

            using (var scope = new UnitOfWorkScope())
            {
                // Prepare data
                var account1 = new BankAccount("001", "Mouhong", 500);
                var account2 = new BankAccount("002", "Shuige");

                scope.UnitOfWork.Save(account1);
                scope.UnitOfWork.Save(account2);

                // Play with domain model
                var service = new MoneyTransferService(scope.UnitOfWork);
                service.Transfer(account1, account2, 100);

                // Complete the UnitOfWorkScope (commit)
                scope.Complete();
            }

            Console.WriteLine();
            Console.WriteLine("[Main] Press any key to exit..");
            Console.WriteLine();
            Console.ReadKey();
        }

        static void AppBootstrap()
        {
            TaroEnvironment.Configure(taro =>
            {
                taro.UsingUnitOfWorkFactory(() => new UnitOfWork());
                taro.UsingDefaultEventDispatcher(typeof(Program).Assembly);
            });
        }
    }
}
