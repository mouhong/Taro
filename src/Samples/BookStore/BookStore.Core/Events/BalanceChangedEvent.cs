using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro;
using System.Runtime.Serialization;

namespace BookStore.Events
{
    [DataContract]
    public class BalanceChangedEvent : Event
    {
        [DataMember]
        public string AccountId { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Message { get; set; }

        public BalanceChangedEvent()
        {
        }

        public BalanceChangedEvent(string accountId, decimal amount, string message)
        {
            AccountId = accountId;
            Amount = amount;
            Message = message;
        }
    }
}
