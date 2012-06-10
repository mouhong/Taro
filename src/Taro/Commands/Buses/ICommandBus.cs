using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Commands.Buses
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand cmd) where TCommand : ICommand;
    }
}
