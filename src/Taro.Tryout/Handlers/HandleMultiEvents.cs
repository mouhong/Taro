using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Events;

namespace Taro.Tryout.Handlers
{
    class HandleMultiEvents : IHandle<OrderPayedEvent>, IHandle<OrderDeliveredEvent>
    {
        public void Handle(OrderPayedEvent evnt)
        {
            Console.WriteLine("Multi: Order payed");
        }

        public void Handle(OrderDeliveredEvent evnt)
        {
            Console.WriteLine("Multi: Order delivered");
        }
    }
}
