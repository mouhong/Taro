using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Handlers
{
    class MultiEventHandler : IHandle<AccountBalanceChanged>, IHandle<MoneyTransferCompleted>
    {
        public void Handle(AccountBalanceChanged evnt)
        {
            Console.WriteLine("#" + evnt.Account.Id + ": Balance changed (MultiEventHandler)");
        }

        public void Handle(MoneyTransferCompleted evnt)
        {
            Console.WriteLine("System: Money transfer completed (MultiEventHandler)");
        }
    }
}
