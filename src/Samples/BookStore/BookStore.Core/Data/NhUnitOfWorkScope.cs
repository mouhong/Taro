using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Data
{
    public class NhUnitOfWorkScope : Taro.Data.UnitOfWorkScope
    {
        public new NhUnitOfWork UnitOfWork
        {
            get
            {
                return (NhUnitOfWork)base.UnitOfWork;
            }
        }

        public NhUnitOfWorkScope()
        {
        }

        public NhUnitOfWorkScope(NhUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
