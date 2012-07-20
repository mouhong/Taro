using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore
{
    public class AccountNotificationSettings
    {
        public virtual bool Enabled { get; protected set; }

        public virtual decimal MinAmount { get; set; }

        public AccountNotificationSettings()
        {
            Enabled = true;
            MinAmount = 100;
        }

        public virtual void Enable()
        {
            if (!Enabled)
            {
                Enabled = true;
            }
        }

        public virtual void Disable()
        {
            if (Enabled)
            {
                Enabled = false;
            }
        }
    }
}
