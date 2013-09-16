using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Tryout.Domain.Events;

namespace Taro.Tryout.Domain
{
    public class Order
    {
        public string Id { get; private set; }

        public decimal Subtotal
        {
            get
            {
                return Items.Sum(x => x.Subtotal);
            }
        }

        public string PaymentMethod { get; private set; }

        public OrderState State { get; private set; }

        public OrderItemCollection Items { get; private set; }

        public Customer Customer { get; private set; }

        public Order(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            Id = Guid.NewGuid().ToString();
            Items = new OrderItemCollection(this);
            Customer = customer;
        }

        public void AcceptPayment(string paymentMethod)
        {
            if (String.IsNullOrEmpty(paymentMethod))
                throw new ArgumentException("'paymentMethod' is required.");

            Console.WriteLine("[Domain] Accept Payment");

            PaymentMethod = paymentMethod;
            State = OrderState.Payed;

            DomainEvent.Apply(new OrderPayed(this));
        }

        public void Deliver()
        {
            if (State != OrderState.Payed)
                throw new InvalidOperationException("Invalid state. Only a payed order can be delivered.");

            Console.WriteLine("[Domain] Delivery");

            State = OrderState.Delivered;

            DomainEvent.Apply(new OrderDelivered(this));
        }

        public void Complete()
        {
            if (State != OrderState.Delivered)
                throw new InvalidOperationException("Invalid state. Only a delivered order can be completed.");

            Console.WriteLine("[Domain] Complete order");

            State = OrderState.Completed;

            DomainEvent.Apply(new OrderCompleted(this));
        }

        public void CreateInvoice()
        {
            Console.WriteLine("[Domain] Creating Invoice");
            DomainEvent.Apply(new InvoiceCreated(this));
        }

        public void LogCustomerCredit()
        {
            Console.WriteLine("[Domain] Log customer credit");
            DomainEvent.Apply(new CustomerCreditLogged(this));
        }
    }
}
