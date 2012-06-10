using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Serialization
{
    public interface ISerializer
    {
        string Serialize(object obj);

        object Deserialize(string serializedObject, Type type);
    }
}
