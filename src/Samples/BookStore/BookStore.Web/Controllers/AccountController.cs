using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using NHibernate.Linq;
using BookStore.Web.Models;
using BookStore.Domain.Services;
using BookStore.Domain;

namespace BookStore.Web.Controllers
{
    public class AccountController : ControllerBase
    {
        [Authorize]
        public ActionResult My()
        {
            var user = Query<BookStore.Domain.User>().Find(User.Identity.Name);
            return View(user);
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model)
        {
            if (ModelState.IsValid)
            {
                var service = new AuthenticationService(CurrentUnitOfWork.Session);

                if (service.Authenticate(model.Email, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel command)
        {
            var service = new RegistrationService(CurrentUnitOfWork.Session);
            service.Register(command.Email, command.NickName, command.Password, command.Gender);

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var user = Query<BookStore.Domain.User>().Find(User.Identity.Name);
            user.ChangePassword(model.NewPassword);

            return RedirectToAction("ChangePasswordSuccess");
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
    }
}
