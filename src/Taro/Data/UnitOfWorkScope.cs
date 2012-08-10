using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Utils;

namespace Taro.Data
{
    public class UnitOfWorkScope : IDisposable
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public UnitOfWorkScope()
            : this(TaroEnvironment.Instance.UnitOfWorkFactory())
        {
        }

        public UnitOfWorkScope(IUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");

            UnitOfWork = unitOfWork;
            ThreadStaticUnitOfWorkContext.Bind(unitOfWork);
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
            ThreadStaticUnitOfWorkContext.Unbind();
        }
    }
}
