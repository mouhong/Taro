using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Events;

namespace Taro.Tryout
{
    public enum OrderState
    {
        Created = 0,
        Payed = 1,
        Delivered = 2,
        Completed = 3
    }

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

    public class Order
    {
        public int Id { get; set; }

        public decimal Subtotal
        {
            get
            {
                return Items.Sum(x => x.Subtotal);
            }
        }

        public string PaymentMethod { get; private set; }

        public OrderState State { get; private set; }

        public IList<OrderItem> Items { get; private set; }

        public Order()
        {
            Items = new List<OrderItem>();
        }

        public void AcceptPayment(string paymentMethod)
        {
            PaymentMethod = paymentMethod;
            State = OrderState.Payed;
            DomainEvent.Apply(new OrderPayedEvent(this, paymentMethod));
        }

        public void Deliver()
        {
            DomainEvent.Apply(new OrderDeliveredEvent(this));
        }
    }
}
