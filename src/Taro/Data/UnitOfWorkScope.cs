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
            : this(UnitOfWorkFactory.Get())
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
