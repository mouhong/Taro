using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Web
{
    public class RequireAdminAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            if (!httpContext.Request.IsAuthenticated || !UserUtil.IsAdmin(httpContext.User.Identity.Name))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
    }
}