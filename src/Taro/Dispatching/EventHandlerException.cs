using System;

namespace Taro.Dispatching
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
