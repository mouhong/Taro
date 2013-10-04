using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Data
{
    public interface IUnitOfWorkAware : IUnitOfWorkAware<UnitOfWork>
    {
        new UnitOfWork UnitOfWork { get; set; }
    }
}
