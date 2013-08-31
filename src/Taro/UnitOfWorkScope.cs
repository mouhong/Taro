using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Config;

namespace Taro
{
    public class UnitOfWorkScope : IDisposable
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkScope()
            : this(TaroEnvironment.Instance.CreateUnitOfWork())
        {
        }

        public UnitOfWorkScope(IUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");

            UnitOfWork = unitOfWork;
            UnitOfWorkAmbient.Bind(unitOfWork);
        }

        public void Commit()
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
