using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Tryout.Domain
{
    public class CustomerAccount : AggregateRoot
    {
        public string Id { get; set; }

        public decimal Balance { get; set; }
    }
}
