using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Domain.Events
{
    public class AccountBalanceChanged : DomainEvent
    {
        public BankAccount Account { get; private set; }

        public decimal Amount { get; private set; }

        public AccountBalanceChanged(BankAccount account, decimal amount)
        {
            Account = account;
            Amount = amount;
        }
    }
}
