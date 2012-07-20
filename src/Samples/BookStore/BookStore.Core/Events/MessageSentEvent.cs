using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using Taro;

namespace BookStore.Events
{
    [DataContract]
    public class MessageSentEvent : Event
    {
        [DataMember]
        public string ReceiverId { get; set; }

        [DataMember]
        public string Content { get; set; }

        public MessageSentEvent()
        {
        }

        public MessageSentEvent(string receiverId, string content)
        {
            ReceiverId = receiverId;
            Content = content;
        }
    }
}
