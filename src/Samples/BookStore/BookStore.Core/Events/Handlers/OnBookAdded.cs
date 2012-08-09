using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Taro.Data;
using Taro.Events;

using BookStore.Events;
using BookStore.Services;

namespace BookStore.Events.Handlers
{
    class OnBookAdded_SendMessage : ImmediatelyEventHandler<BookAddedEvent>
    {
        public override void Handle(BookAddedEvent evnt)
        {
            var service = new MessageService(UnitOfWork.Session);

            var users = UnitOfWork.Query<User>()
                                  .Where(it => it.Id != evnt.CreatorId)
                                  .Select(it => new
                                  {
                                      it.Id,
                                      it.NickName
                                  });

            foreach (var user in users)
            {
                var message = "Hello {0}, A new book \"{1}\" has arrived. Don't want to have a look?";
                message = String.Format(message, user.NickName, evnt.Title);

                service.Send(user.Id, message);
            }
        }
    }

    class OnBookAdded_UpdateStatistics : ImmediatelyEventHandler<BookAddedEvent>
    {
        public override void Handle(BookAddedEvent evnt)
        {
            var service = new WebsiteInfoService(UnitOfWork.Session);
            var totalBooks = Convert.ToInt32(service.GetValue(WebsiteInfoKeys.TotalBooks, "0"));
            service.SetValue(WebsiteInfoKeys.TotalBooks, (totalBooks + 1).ToString());
        }
    }
}
