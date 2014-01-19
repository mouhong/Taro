using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Handlers
{
    class BaseEventHandler : IHandle<IEvent>
    {
        public void Handle(IEvent evnt)
        {
            Console.WriteLine("System: Event raised (BaseEventHandler)");
        }
    }
}
