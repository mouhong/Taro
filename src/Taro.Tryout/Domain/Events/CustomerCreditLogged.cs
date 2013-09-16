using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class CustomerCreditLogged : DomainEvent
    {
        public Order Order { get; private set; }

        public CustomerCreditLogged(Order order)
        {
            Order = order;
        }
    }
}
