using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    static class Require
    {
        public static void That(bool condition, string message)
        {
            if (!condition)
                throw new InvalidOperationException(message);
        }

        public static void NotNull(object param, string paramName)
        {
            if (param == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
