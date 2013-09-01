using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class AwardPointsChanged : DomainEvent
    {
        public Customer Customer { get; private set; }

        public int Amount { get; private set; }

        public AwardPointsChanged(Customer customer, int amount)
        {
            Customer = customer;
            Amount = amount;
        }
    }
}
