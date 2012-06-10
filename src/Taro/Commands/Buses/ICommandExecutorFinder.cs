using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Taro.Commands.Buses
{
    public interface ICommandExecutorFinder
    {
        IExecuteCommand<TCommand> FindExecutor<TCommand>()
            where TCommand : ICommand;
    }
}
