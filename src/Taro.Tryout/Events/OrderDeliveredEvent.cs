using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Events
{
    public class OrderDeliveredEvent : DomainEvent
    {
        public Order Order { get; private set; }

        public OrderDeliveredEvent(Order order)
        {
            Order = order;
        }
    }
}
