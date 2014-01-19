using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public interface IHandle<TEvent>
        where TEvent: IEvent
    {
        void Handle(TEvent evnt);
    }
}
