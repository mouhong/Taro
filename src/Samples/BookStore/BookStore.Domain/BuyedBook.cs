using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Domain
{
    public class BuyedBook
    {
        public virtual string Id { get; protected set; }

        public virtual User Owner { get; protected set; }

        public virtual Book Book { get; protected set; }

        public virtual decimal Price { get; protected set; }

        public virtual DateTime BuyTime { get; protected set; }

        protected BuyedBook()
        {
        }

        public BuyedBook(User owner, Book book)
        {
            Id = Guid.NewGuid().ToString();
            Owner = owner;
            Book = book;
            Price = book.Price;
            BuyTime = DateTime.Now;
        }
    }
}
