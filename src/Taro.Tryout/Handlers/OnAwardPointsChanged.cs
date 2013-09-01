using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    [AwaitCommitted, HandleAsync]
    class OnAwardPointsChanged : IHandle<AwardPointsChanged>
    {
        public void Handle(AwardPointsChanged evnt)
        {
            Console.WriteLine("[Mail] To customer: Congratulations! You've earned " + evnt.Amount + " award points!");
            Thread.Sleep(3000);
            Console.WriteLine("[Mail] Award points change notification succeeded");
        }
    }
}
