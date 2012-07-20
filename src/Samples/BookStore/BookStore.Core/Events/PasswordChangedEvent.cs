using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using Taro;

namespace BookStore.Events
{
    [DataContract]
    public class PasswordChangedEvent : Event
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string NewPassword { get; set; }

        public PasswordChangedEvent()
        {
        }

        public PasswordChangedEvent(string userId, string newPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
        }
    }
}
