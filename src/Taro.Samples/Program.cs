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

            using(var unitOfWork = new UnitOfWork())
            using (EventContext.Begin(unitOfWork))
            {
                // Prepare data
                var account1 = new BankAccount("001", "Mouhong", 500);
                var account2 = new BankAccount("002", "Shuige");

                unitOfWork.Save(account1);
                unitOfWork.Save(account2);

                // Play with domain model
                var service = new MoneyTransferService(unitOfWork);
                service.Transfer(account1, account2, 100);

                unitOfWork.Commit();
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
                taro.UsingDefaultEventDispatcher(typeof(Program).Assembly);
            });
        }
    }
}
