using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Persistence
{
    public interface IStoredEvent
    {
        IEvent Unwrap();
    }
}
