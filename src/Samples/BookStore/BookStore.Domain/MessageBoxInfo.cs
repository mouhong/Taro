using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Domain
{
    public class MessageBoxInfo
    {
        public virtual string UserId { get; protected set; }

        public virtual int TotalMessages { get; set; }

        public virtual int UnReadMessages { get; set; }

        protected MessageBoxInfo()
        {
        }

        public MessageBoxInfo(string userId)
        {
            UserId = userId;
        }
    }
}
