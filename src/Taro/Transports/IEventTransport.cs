﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Transports
{
    public interface IEventTransport
    {
        void Send(IEvent @event);
    }
}
