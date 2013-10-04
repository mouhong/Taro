using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Handlers
{
    // [HandleAsync], attributes can also be attached here
    class AsyncHandler : IHandle<MoneyTransferCompleted>
    {
        [HandleAsync]
        public void Handle(MoneyTransferCompleted evnt)
        {
            Thread.Sleep(500);
            Console.WriteLine("Mail Service: Money transfer completed, sending mail to " + evnt.SourceAccount.Owner);
            Thread.Sleep(1000);
            Console.WriteLine("Mail Service: Finish sending mail to " + evnt.SourceAccount.Owner);
        }
    }
}
