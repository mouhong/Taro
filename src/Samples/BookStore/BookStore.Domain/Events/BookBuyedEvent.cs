using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro;
using System.Runtime.Serialization;

namespace BookStore.Domain.Events
{
    [DataContract]
    public class BookBuyedEvent : Event
    {
        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public string BookISBN { get; set; }

        [DataMember]
        public string BookTitle { get; set; }

        [DataMember]
        public DateTime BuyTime { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        public BookBuyedEvent()
        {
        }

        public BookBuyedEvent(string userId, string bookISBN, string bookTitle, DateTime buyTime, decimal price)
        {
            UserId = userId;
            BookISBN = bookISBN;
            BookTitle = bookTitle;
            BuyTime = buyTime;
            Price = price;
        }
    }
}
