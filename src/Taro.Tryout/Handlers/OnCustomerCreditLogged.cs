using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Handlers
{
    class OnCustomerCreditLogged : IHandle<CustomerCreditLogged>
    {
        public void Handle(CustomerCreditLogged evnt)
        {
            Console.WriteLine("[BeforeCommit] On customer credit logged");
        }
    }
}
