using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class OrderDelivered : DomainEvent
    {
        public Order Order { get; private set; }

        public OrderDelivered(Order order)
        {
            Order = order;
        }
    }
}
