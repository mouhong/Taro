using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Commands.Buses
{
    public class DefaultCommandBus : ICommandBus
    {
        private ICommandExecutorFinder _executorFinder;

        public DefaultCommandBus(ICommandExecutorFinder executorFinder)
        {
            _executorFinder = executorFinder;
        }

        public void Send<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            var executor = _executorFinder.FindExecutor<TCommand>();
            executor.Execute(cmd);
        }
    }
}
