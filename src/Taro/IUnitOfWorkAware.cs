using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro
{
    public interface IUnitOfWorkAware
    {
        IUnitOfWork UnitOfWork { get; set; }
    }
}
