using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;

using BookStore.Domain.Events;

namespace BookStore.Domain
{
    public class Book
    {
        public virtual string ISBN { get; set; }

        public virtual string Title { get; set; }

        public virtual string Author { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int Stock { get; set; }

        public virtual DateTime PublishedDate { get; set; }

        public virtual string CreatorId { get; set; }

        protected Book()
        {
            Stock = 100;
            PublishedDate = DateTime.Now;
        }

        public static Book Create(string isbn, string title, string author, DateTime publishedDate, decimal price, int stock, string creatorId)
        {
            var book = new Book
            {
                ISBN = isbn,
                Title = title,
                Author = author,
                Price = price,
                Stock = stock,
                PublishedDate = publishedDate,
                CreatorId = creatorId
            };

            DomainEvent.Apply(new BookAddedEvent(isbn, title, creatorId));

            return book;
        }
    }
}
