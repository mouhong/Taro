using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class OrderDelivered : StateChanged
    {
        public OrderDelivered(Order order)
            : base(order)
        {
        }
    }
}
