using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Commands
{
    public interface IExecuteCommand<TCommand>
        where TCommand : ICommand
    {
        void Execute(TCommand cmd);
    }
}
