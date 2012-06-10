using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Domain
{
    public class BookSalesCounter
    {
        public virtual string ISBN { get; protected set; }

        public virtual string Title { get; protected set; }

        public virtual int TotalSoldCount { get; set; }

        protected BookSalesCounter()
        {
        }

        public BookSalesCounter(string isbn, string title)
        {
            ISBN = isbn;
            Title = title;
        }
    }
}
