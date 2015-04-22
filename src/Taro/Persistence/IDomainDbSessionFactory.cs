using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Persistence
{
    public interface IDomainDbSessionFactory
    {
        IDomainDbSession OpenSession();
    }
}
