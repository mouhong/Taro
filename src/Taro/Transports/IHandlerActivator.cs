using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Transports
{
    public interface IHandlerActivator
    {
        object CreateInstance(Type handlerType);
    }
}
