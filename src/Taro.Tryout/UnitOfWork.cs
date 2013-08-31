using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout
{
    public class UnitOfWork : AbstractUnitOfWork
    {
        protected override void DoCommit()
        {
            Console.WriteLine("Unit of Work: Committed");
        }
    }
}
