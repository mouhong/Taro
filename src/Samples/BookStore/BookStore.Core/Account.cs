using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro;
using BookStore.Events;

namespace BookStore
{
    public class Account
    {
        public virtual string Id { get; protected set; }

        public virtual decimal Balance { get; protected set; }

        public virtual AccountNotificationSettings NotificationSettings { get; protected set; }

        public virtual IList<AccountLog> Logs { get; protected set; }

        public Account()
        {
            Id = Guid.NewGuid().ToString();
            Balance = 1000;
            NotificationSettings = new AccountNotificationSettings();
            Logs = new List<AccountLog>();
        }

        public virtual void Increase(decimal amount, string message)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount should be greater than 0.", "amount");

            Balance += amount;
            Logs.Add(new AccountLog(Id, amount, message));

            DomainEvent.Apply(new BalanceChangedEvent(Id, amount, message));
        }

        public virtual void Decrease(decimal amount, string message)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount should be greater than 0.", "amount");

            if (Balance < amount)
                throw new InvalidOperationException("Balance not enought.");

            Balance -= amount;
            Logs.Add(new AccountLog(Id, -amount, message));

            DomainEvent.Apply(new BalanceChangedEvent(Id, -amount, message));
        }
    }
}
