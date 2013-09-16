using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    class OnOrderStateChanged : IHandle<StateChanged>
    {
        public void Handle(StateChanged evnt)
        {
            Console.WriteLine("[BeforeCommit] State changed to " + evnt.NewState);
        }
    }
}
