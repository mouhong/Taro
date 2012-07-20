using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Domain;
using BookStore.Domain.Services;

namespace BookStore.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var bestSoldBooks = Query<BookSalesCounter>()
                                .OrderByDescending(it => it.TotalSoldCount)
                                .Take(6)
                                .ToList();

            ViewBag.BestSoldBooks = bestSoldBooks;

            return View();
        }

        public ActionResult Stat()
        {
            var service = new WebsiteInfoService(CurrentUnitOfWork.Session);

            ViewBag.TotalBooks = service.GetValue(WebsiteInfoKeys.TotalBooks, "0");
            ViewBag.TotalUsers = service.GetValue(WebsiteInfoKeys.TotalUsers, "0");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
