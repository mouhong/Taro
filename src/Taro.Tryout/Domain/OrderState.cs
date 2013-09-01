using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain
{
    public enum OrderState
    {
        Created = 0,
        Payed = 1,
        Delivered = 2,
        Completed = 3
    }
}
