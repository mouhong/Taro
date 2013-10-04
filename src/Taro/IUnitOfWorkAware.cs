using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public interface IUnitOfWorkAware<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork
    {
        TUnitOfWork UnitOfWork { get; set; }
    }
}
