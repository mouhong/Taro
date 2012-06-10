using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tests.Domain.Events
{
    public class AValueAdded : Event
    {
        public int ValueToAdd { get; set; }
    }
}
