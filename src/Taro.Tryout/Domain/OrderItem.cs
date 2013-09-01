using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal Subtotal
        {
            get
            {
                return UnitPrice * Quantity;
            }
        }
    }
}
