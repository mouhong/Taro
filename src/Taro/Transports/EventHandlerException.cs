using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Transports
{
    [Serializable]
    public class EventHandlerException : Exception
    {
        public EventHandlerException() { }
        public EventHandlerException(string message) : base(message) { }
        public EventHandlerException(string message, Exception inner) : base(message, inner) { }
        protected EventHandlerException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
