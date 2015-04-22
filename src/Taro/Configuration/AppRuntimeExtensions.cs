using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Configuration
{
    public static class AppRuntimeExtensions
    {
        public static T GetItem<T>(this AppRuntime runtime)
        {
            var item = runtime.Items[typeof(T).Name];
            if (item == null)
            {
                return default(T);
            }

            return (T)item;
        }

        public static void SetItem<T>(this AppRuntime runtime, T item)
        {
            runtime.Items[typeof(T).Name] = item;
        }
    }
}
