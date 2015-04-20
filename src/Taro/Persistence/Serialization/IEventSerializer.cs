using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence.Serialization
{
    public interface IEventSerializer<T>
    {
        T Serialize(IEvent theEvent);

        IEvent Deserialize(T obj);
    }
}
