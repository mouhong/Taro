using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Tryout.Events;

namespace Taro.Tryout.Handlers
{
    class WhenOrderWasPayed : IHandle<OrderPayedEvent>
    {
        public void Handle(OrderPayedEvent evnt)
        {
            Console.WriteLine("Unit of Work: Order " + evnt.Order.Id + " was paid.");
        }
    }

    [AwaitCommitted]
    class WhenOrderWasPayedAndCommitted_InvokeOneShortOperation : IHandle<OrderPayedEvent>
    {
        public void Handle(OrderPayedEvent evnt)
        {
            Console.WriteLine("After Commit: Short operation");
        }
    }

    [AwaitCommitted, HandleAsync]
    class WhenOrderWasPayedAndComitted_InvokeOneLongOperation : IHandle<OrderPayedEvent>
    {
        public void Handle(OrderPayedEvent evnt)
        {
            Console.WriteLine("After Commit: Start sending email to customer.");
            Thread.Sleep(3000);
            Console.WriteLine("After Commit: Finish sending email to customer (3 seconds).");
        }
    }
}
