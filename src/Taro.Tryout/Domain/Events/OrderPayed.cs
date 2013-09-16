using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain.Events
{
    public class OrderPayed : StateChanged
    {
        public OrderPayed(Order order)
            : base(order)
        {
        }
    }
}
