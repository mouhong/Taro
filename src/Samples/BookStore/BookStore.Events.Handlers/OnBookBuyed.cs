using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using Taro.Events;

using BookStore.Domain;
using BookStore.Domain.Events;

namespace BookStore.Events.Handlers
{
    class OnBookBuyed : HandlesImmediately<BookBuyedEvent>
    {
        public override void Handle(BookBuyedEvent evnt)
        {
            var counter = UnitOfWork.Get<BookSalesCounter>(evnt.BookISBN);

            if (counter == null)
            {
                counter = new BookSalesCounter(evnt.BookISBN, evnt.BookTitle);
                UnitOfWork.Save(counter);
            }

            counter.TotalSoldCount++;
        }
    }
}
