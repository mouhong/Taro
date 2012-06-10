using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using Taro.Utils;

namespace Taro.Commands
{
    public abstract class ExecuteCommand<TCommand> : IExecuteCommand<TCommand>
        where TCommand : ICommand
    {
        protected Func<IUnitOfWork> GetUnitOfWork { get; private set; }

        protected ExecuteCommand()
            : this(UnitOfWorkFactory.Get)
        {
        }

        protected ExecuteCommand(Func<IUnitOfWork> getUnitOfWork)
        {
            Require.NotNull(getUnitOfWork, "getUnitOfWork");

            GetUnitOfWork = getUnitOfWork;
        }

        public void Execute(TCommand cmd)
        {
            using (var unitOfWork = GetUnitOfWork())
            {
                if (unitOfWork == null)
                    throw new InvalidOperationException("Unit of work factory cannot return null. Please ensure that you have setup Taro.Data.UnitOfWorkFactory.Create.");

                ThreadStaticUnitOfWorkContext.Bind(unitOfWork);

                try
                {
                    Execute(unitOfWork, cmd);
                }
                finally
                {
                    ThreadStaticUnitOfWorkContext.Unbind();
                }
            }
        }

        protected abstract void Execute(IUnitOfWork unitOfWork, TCommand cmd);
    }
}
