using System;

namespace Taro
{
    static class Require
    {
        public static void NotNull(object param, string paramName)
        {
            if (param == null)
                throw new ArgumentNullException(paramName);
        }

        public static void NotNullOrEmpty(string param, string paramName)
        {
            if (String.IsNullOrEmpty(param))
                throw new ArgumentException(String.Format("{0} cannot be null or empty.", paramName));
        }

        public static void NotNullOrWhitespace(string param, string paramName)
        {
            if (String.IsNullOrWhiteSpace(param))
                throw new ArgumentException(String.Format("{0} cannot be null or whitespace.", paramName));
        }
    }
}
