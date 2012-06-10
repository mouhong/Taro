using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;

namespace Taro.Commands
{
    public abstract class Executes<TCommand> : ExecuteCommand<TCommand>
        where TCommand : ICommand
    {
        protected Executes()
        {
        }

        protected override void Execute(IUnitOfWork unitOfWork, TCommand cmd)
        {
            Execute((UnitOfWork)unitOfWork, cmd);
        }

        protected abstract void Execute(UnitOfWork unitOfWork, TCommand cmd);
    }
}
