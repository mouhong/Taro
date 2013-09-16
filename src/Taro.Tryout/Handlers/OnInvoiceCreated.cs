using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Data;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    class OnInvoiceCreated : IHandle<InvoiceCreated>
    {
        public void Handle(InvoiceCreated evnt)
        {
            Console.WriteLine("[BeforeCommit] Invoice created");
        }
    }

    [AwaitCommitted]
    class OnInvoiceCreated_Mail : IHandle<InvoiceCreated>
    {
        public void Handle(InvoiceCreated evnt)
        {
            Console.WriteLine("[AfterCommit] Send invoice to customer");

            using (var scope = new UnitOfWorkScope())
            {
                Console.WriteLine("Start Nested Unit of Work scope #2");
                evnt.Order.LogCustomerCredit();
                Console.WriteLine("End Nested Unit of Work scope #2");
            }
        }
    }
}
