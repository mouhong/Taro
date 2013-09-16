using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class InvoiceCreated : DomainEvent
    {
        public Order Order { get; private set; }

        public InvoiceCreated(Order order)
        {
            Order = order;
        }
    }
}
