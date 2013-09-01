using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class OrderCompleted : DomainEvent
    {
        public Order Order { get; private set; }

        public OrderCompleted(Order order)
        {
            Order = order;
        }
    }
}
