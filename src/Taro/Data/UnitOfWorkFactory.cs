using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Data
{
    public static class UnitOfWorkFactory
    {
        public static Func<IUnitOfWork> Get;
    }
}
