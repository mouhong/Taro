using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Tryout.Domain;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    // AwaitCommitted: This handler will be executed only when the unit of work is committed successfully
    // HandleAsync: This handler will be executed in async manner
    [AwaitCommitted, HandleAsync]
    class OnOrderPayed_MailCustomer : IHandle<OrderPayed>
    {
        public void Handle(OrderPayed evnt)
        {
            Console.WriteLine("[Mail] To customer: Thank you for choosing us!");
            Thread.Sleep(3000);
            Console.WriteLine("[Mail] Payment notification succeeded (to customer)");
        }
    }

    [AwaitCommitted, HandleAsync]
    class OnOrderPayed_MailCustomerService : IHandle<OrderPayed>
    {
        public void Handle(OrderPayed evnt)
        {
            Console.WriteLine("[Mail] To customer service: New order! Please prepare for delivery.");
            Thread.Sleep(3000);
            Console.WriteLine("[Mail] Payment notification succeeded (to customer service)");
        }
    }
}
