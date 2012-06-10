using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BookStore.Domain;
using BookStore.Commands;
using BookStore.Domain.Services;

namespace BookStore.Web.Controllers
{
    public class BookController : ControllerBase
    {
        public ActionResult Index()
        {
            return All();
        }

        public ActionResult All(int page = 1, string message = null, bool isError = false)
        {
            ViewBag.Message = message;
            ViewBag.IsError = isError;

            var pageSize = 10;
            var books = Query<Book>()
                        .OrderByDescending(it => it.PublishedDate)
                        .Skip(pageSize * (page - 1))
                        .Take(pageSize)
                        .ToList();

            ViewBag.Count = Query<Book>().Count();
            ViewBag.PageNumber = page;
            ViewBag.PageCount = (ViewBag.Count / pageSize + (ViewBag.Count % pageSize > 0 ? 1 : 0));

            return View(books);
        }

        [RequireAdmin]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, RequireAdmin]
        public ActionResult Create(AddBookCommand command)
        {
            var service = new UserService(NhSession);
            var creatorId = service.GetUserIdByEmail(User.Identity.Name);
            command.CreatorId = creatorId;

            CommandBus.Send(command);

            return RedirectToAction("All", new { message = "Book created!" });
        }

        [Authorize]
        public ActionResult Buy(string isbn)
        {
            var cmd = new BuyBookCommand(User.Identity.Name, isbn);

            try
            {
                CommandBus.Send(cmd);
                return RedirectToAction("All", new { message = "Buyed one book" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("All", new { message = ex.Message, isError = true });
            }
        }
    }
}
