using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using Taro;

namespace BookStore.Domain.Events
{
    [DataContract]
    public class NewUserRegisteredEvent : Event
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string UserEmail { get; set; }

        [DataMember]
        public string UserNickName { get; set; }

        public NewUserRegisteredEvent()
        {
        }
    }
}
