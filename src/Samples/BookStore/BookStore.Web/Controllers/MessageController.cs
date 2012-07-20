using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate.Linq;

using BookStore.Domain;
using BookStore.Domain.Services;
using BookStore.Commands;

namespace BookStore.Web.Controllers
{
    public class MessageController : ControllerBase
    {
        [Authorize]
        public ActionResult Index()
        {
            var user = CurrentUnitOfWork.Session.Query<User>().Find(User.Identity.Name);

            var service = new MessageService(CurrentUnitOfWork.Session);
            service.MarkAllMessagesAsRead(user.Id);

            var messages = service.QueryMessages(user.Id)
                                  .OrderByDescending(it => it.SentTime)
                                  .ToList();

            return View(messages);
        }

        public ActionResult UnReadMessageCountHtml()
        {
            if (Request.IsAuthenticated)
            {
                var user = Query<User>().Find(User.Identity.Name);

                var messageService = new MessageService(CurrentUnitOfWork.Session);
                var messageCount = messageService.GetTotalUnReadMessageCount(user.Id);

                if (messageCount > 0)
                {
                    return Content("(" + messageCount.ToString() + ")");
                }
            }

            return Content(String.Empty);
        }
    }
}
