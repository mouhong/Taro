using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    [AwaitCommitted, HandleAsync]
    class OnOrderDelivered : IHandle<OrderDelivered>
    {
        public void Handle(OrderDelivered evnt)
        {
            Console.WriteLine("[Mail] To customer: Your order is delivered ^^");
            Thread.Sleep(3000);
            Console.WriteLine("[Mail] Delivery notification succeeded (to customer)");
        }
    }
}
