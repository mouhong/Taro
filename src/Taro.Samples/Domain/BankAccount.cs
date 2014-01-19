using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Domain
{
    public class BankAccount
    {
        public string Id { get; private set; }

        public string Owner { get; private set; }

        public decimal Balance { get; private set; }

        public BankAccount(string id, string owner)
            : this(id, owner, 0)
        {
        }

        public BankAccount(string id, string owner, decimal initialBalance)
        {
            Id = id;
            Owner = owner;
            Balance = initialBalance;
        }

        public void Increase(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("'amount' should be greater than 0.");

            Balance += amount;

            Event.Apply(new AccountBalanceChanged(this, amount));
        }

        public void Decrease(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("'amount' should be greater than 0.");

            if (Balance < amount)
                throw new InvalidOperationException("Balance not enough.");

            Balance -= amount;

            Event.Apply(new AccountBalanceChanged(this, -amount));
        }
    }
}
