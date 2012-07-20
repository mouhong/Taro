using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore
{
    public class AccountLog
    {
        public virtual string Id { get; protected set; }

        public virtual string AccountId { get; protected set; }

        public virtual string Message { get; set; }

        public virtual decimal Amount { get; protected set; }

        public virtual DateTime LogTime { get; protected set; }

        protected AccountLog()
        {
        }

        public AccountLog(string accountId, decimal amount, string message)
        {
            Id = Guid.NewGuid().ToString();
            AccountId = accountId;
            Amount = amount;
            Message = message;
            LogTime = DateTime.Now;
        }
    }
}
