using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Samples.Data;
using Taro.Samples.Domain;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Handlers
{
    class PreCommitHandler : IHandle<AccountBalanceChanged>
    {
        public void Handle(AccountBalanceChanged evnt)
        {
            Console.WriteLine("#" + evnt.Account.Id + ": Balance changed (PreCommit)");
        }
    }
}
