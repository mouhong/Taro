using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Tryout.Domain.Events
{
    public class CustomerApproved : Event
    {
        public string CustomerId { get; set; }
    }
}
