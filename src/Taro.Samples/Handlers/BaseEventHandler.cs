using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Handlers
{
    class BaseEventHandler : IHandle<DomainEvent>
    {
        public void Handle(DomainEvent evnt)
        {
            Console.WriteLine("System: Event raised (BaseEventHandler)");
        }
    }
}
