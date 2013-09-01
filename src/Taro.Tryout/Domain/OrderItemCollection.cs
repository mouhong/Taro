using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Tryout.Domain
{
    public class OrderItemCollection : IEnumerable<OrderItem>
    {
        private Order _order;
        private List<OrderItem> _items = new List<OrderItem>();

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public OrderItemCollection(Order order)
        {
            _order = order;
        }

        public void Add(OrderItem item)
        {
            _items.Add(item);
        }

        public IEnumerator<OrderItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
