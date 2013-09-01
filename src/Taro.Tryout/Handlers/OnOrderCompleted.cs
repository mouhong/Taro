using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    class OnOrderCompleted : IHandle<OrderCompleted>
    {
        public void Handle(OrderCompleted evnt)
        {
            var customer = evnt.Order.Customer;
            customer.IncreaseAwardPoint((int)evnt.Order.Subtotal);

            Console.WriteLine("[Statistics] Total sales + 1");
        }
    }
}
