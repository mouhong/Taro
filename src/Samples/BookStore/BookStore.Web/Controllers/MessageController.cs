using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            CommandBus.Send(new MarkAllMessagesAsReadCommand(User.Identity.Name));

            var service = new UserService(NhSession);
            var userId = service.GetUserIdByEmail(User.Identity.Name);
            var messages = Query<Message>()
                           .Where(it => it.UserId == userId)
                           .OrderByDescending(it => it.SentTime)
                           .ToList();

            return View(messages);
        }

        public ActionResult UnReadMessageCountHtml()
        {
            if (Request.IsAuthenticated)
            {
                var userService = new UserService(NhSession);
                var userId = userService.GetUserIdByEmail(User.Identity.Name);

                var messageService = new MessageService(NhSession);
                var messageCount = messageService.GetTotalUnReadMessageCount(userId);

                if (messageCount > 0)
                {
                    return Content("(" + messageCount.ToString() + ")");
                }
            }

            return Content(String.Empty);
        }
    }
}
