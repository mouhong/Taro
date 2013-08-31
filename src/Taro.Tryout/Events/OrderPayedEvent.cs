using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Events
{
    public class OrderPayedEvent : DomainEvent
    {
        public Order Order { get; private set; }

        public string PaymentMethod { get; private set; }

        public OrderPayedEvent(Order order, string paymentMethod)
        {
            Order = order;
            PaymentMethod = paymentMethod;
        }
    }
}
