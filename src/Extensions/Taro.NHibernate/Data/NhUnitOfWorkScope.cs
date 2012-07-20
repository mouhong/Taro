using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Data
{
    public class NhUnitOfWorkScope : Taro.Data.UnitOfWorkScope
    {
        public new UnitOfWork UnitOfWork
        {
            get
            {
                return (UnitOfWork)base.UnitOfWork;
            }
        }

        public NhUnitOfWorkScope()
        {
        }

        public NhUnitOfWorkScope(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
