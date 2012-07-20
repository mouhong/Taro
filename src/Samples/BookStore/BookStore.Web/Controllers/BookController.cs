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
            var service = new RegistrationService(CurrentUnitOfWork.Session);
            var creator = CurrentUnitOfWork.Session.Get<BookStore.Domain.User>(User.Identity.Name);

            var book = Book.Create(command.ISBN, command.Title, command.Author, command.PublishedDate, command.Price, command.Stock, creator.Id);
            CurrentUnitOfWork.Save(book);

            return RedirectToAction("All", new { message = "Book created!" });
        }

        [Authorize]
        public ActionResult Buy(string isbn)
        {
            var user = Query<User>().First(it => it.Email == User.Identity.Name);
            var book = CurrentUnitOfWork.Get<Book>(isbn);

            user.BuyBook(book);

            return RedirectToAction("All", new { message = "Buyed one book" });
        }
    }
}
