using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Taro.Samples.Domain.Events
{
    public class MoneyTransferCompleted : IEvent
    {
        public BankAccount SourceAccount { get; private set; }

        public BankAccount DestinationAccount { get; private set; }

        public decimal Amount { get; private set; }

        public MoneyTransferCompleted(BankAccount source, BankAccount dest, decimal amount)
        {
            SourceAccount = source;
            DestinationAccount = dest;
            Amount = amount;
        }
    }
}
