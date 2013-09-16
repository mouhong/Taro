using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class StateChanged : DomainEvent
    {
        public Order Order { get; private set; }

        public OrderState NewState { get; private set; }

        public StateChanged(Order order)
        {
            Order = order;
            NewState = order.State;
        }
    }
}
