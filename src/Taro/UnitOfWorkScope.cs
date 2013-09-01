using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Config;

namespace Taro
{
    public class UnitOfWorkScope<TUnitOfWork> : IDisposable
        where TUnitOfWork : IUnitOfWork
    {
        public TUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkScope()
        {
            var unitOfWork = TaroEnvironment.Instance.CreateUnitOfWork();

            if (!(unitOfWork is TUnitOfWork))
                throw new InvalidOperationException("Unit of work scope requires the unit of work to be of type " + typeof(TUnitOfWork) + ", but the unit of work is " + unitOfWork.GetType() + ".");

            Bind((TUnitOfWork)unitOfWork);
        }

        public UnitOfWorkScope(TUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            Bind(unitOfWork);
        }

        private void Bind(TUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
            UnitOfWorkAmbient.Bind(unitOfWork);
        }

        public void Complete()
        {
            UnitOfWork.Commit();
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            UnitOfWorkAmbient.Unbind();
        }
    }
}
