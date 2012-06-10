using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Serialization
{
    public static class Serializers
    {
        public static Func<ISerializer> Current = () => new DefaultJsonSerializer();
    }
}
