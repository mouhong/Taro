using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Data
{
    public class InMemoryUnitOfWork : UnitOfWorkBase
    {
        protected override void DoCommit()
        {
            Console.WriteLine();
            Console.WriteLine("[Database] Committed");
        }
    }
}
