using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Taro.Events.Buses
{
    public static class EventHandlerInvoker
    {
        public static void Invoke(object handler, object evnt)
        {
            var method = handler.GetType().GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);
            method.Invoke(handler, new object[] { evnt });
        }
    }
}
