using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Tryout.Data;
using Taro.Tryout.Domain;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    // AwaitCommitted: This handler will be executed only when the unit of work is committed successfully
    // HandleAsync: This handler will be executed in async manner
    //[AwaitCommitted, HandleAsync]
    //class OnOrderPayed_MailCustomer : IHandle<OrderPayed>
    //{
    //    public void Handle(OrderPayed evnt)
    //    {
    //        Console.WriteLine("[Mail] To customer: Thank you for choosing us!");
    //        Thread.Sleep(3000);
    //        Console.WriteLine("[Mail] Payment notification succeeded (to customer)");
    //    }
    //}

    //[AwaitCommitted, HandleAsync]
    //class OnOrderPayed_MailCustomerService : IHandle<OrderPayed>
    //{
    //    public void Handle(OrderPayed evnt)
    //    {
    //        Console.WriteLine("[Mail] To customer service: New order! Please prepare for delivery.");
    //        Thread.Sleep(3000);
    //        Console.WriteLine("[Mail] Payment notification succeeded (to customer service)");
    //    }
    //}

    class OnOrderPayed : IHandle<OrderPayed>, IUnitOfWorkAware
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public void Handle(OrderPayed evnt)
        {
            Console.WriteLine("[BeforeCommit] Order payed");
        }
    }

    [AwaitCommitted]
    class OnOrderPayed_CreateInvoice : IHandle<OrderPayed>
    {
        public void Handle(OrderPayed evnt)
        {
            Console.WriteLine("[AfterCommit] Order payed, need to create invoice");

            //evnt.Order.CreateInvoice();

            //var unitOfWork = (InMemoryUnitOfWork)UnitOfWorkAmbient.Current;

            //unitOfWork.Commit();

            //using (var scope = new UnitOfWorkScope())
            //{
            //    Console.WriteLine();
            //    Console.WriteLine("Start Nested Unit of Work Scope");
            //    Console.WriteLine();

            //    evnt.Order.CreateInvoice();
            //    scope.Complete();

            //    Console.WriteLine();
            //    Console.WriteLine("Complete Nested Unit of Work Scope");
            //    Console.WriteLine();
            //}
        }
    }
}
