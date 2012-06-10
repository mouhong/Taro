using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Events.Storage
{
    public interface IEventIdGenerator
    {
        int Generate();
    }
}
