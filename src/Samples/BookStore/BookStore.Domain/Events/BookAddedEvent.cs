using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;

namespace BookStore.Domain.Events
{
    [Serializable]
    public class BookAddedEvent : Event
    {
        public string ISBN { get; set; }

        public string Title { get; set; }

        public string CreatorId { get; set; }

        public BookAddedEvent()
        {
        }

        public BookAddedEvent(string isbn, string title, string creatorId)
        {
            ISBN = isbn;
            Title = title;
            CreatorId = creatorId;
        }
    }
}
