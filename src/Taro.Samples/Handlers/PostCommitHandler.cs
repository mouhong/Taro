using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Handlers
{
    [AwaitCommitted]
    class PostCommitHandler : IHandle<AccountBalanceChanged>
    {
        // [AwaitCommitted], attributes can also be attached here
        public void Handle(AccountBalanceChanged evnt)
        {
            Console.WriteLine("#" +  evnt.Account.Id + ": Balance changed (PostCommit)");
        }
    }
}
