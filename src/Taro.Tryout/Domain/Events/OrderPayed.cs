using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class OrderPayed : DomainEvent
    {
        public Order Order { get; private set; }

        public OrderPayed(Order order)
        {
            Order = order;
        }
    }
}
