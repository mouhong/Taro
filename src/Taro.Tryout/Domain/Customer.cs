using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Domain
{
    public class Customer : AggregateRoot
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int TotalValidOrders { get; set; }

        public bool IsApproved { get; private set; }

        public void Approve()
        {
            if (IsApproved)
            {
                return;
            }

            IsApproved = true;

            AppendEvent(new CustomerApproved
            {
                CustomerId = Id
            });
        }
    }
}
