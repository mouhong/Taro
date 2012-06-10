using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Domain
{
    public class Message
    {
        public virtual string Id { get; protected set; }

        public virtual string Content { get; set; }

        public virtual DateTime SentTime { get; set; }

        public virtual bool IsRead { get; set; }

        public virtual string UserId { get; protected set; }

        protected Message()
        {
        }

        public Message(string userId)
        {
            Id = Guid.NewGuid().ToString();
            SentTime = DateTime.Now;
            UserId = userId;
        }
    }
}
