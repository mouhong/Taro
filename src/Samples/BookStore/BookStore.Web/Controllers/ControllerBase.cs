using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate;
using NHibernate.Linq;

using Taro;
using Taro.Data;

namespace BookStore.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected NhUnitOfWorkScope CurrentUnitOfWorkScope { get; private set; }

        protected UnitOfWork CurrentUnitOfWork { get; private set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            CurrentUnitOfWorkScope = new NhUnitOfWorkScope();
            CurrentUnitOfWork = CurrentUnitOfWorkScope.UnitOfWork;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (filterContext.Exception == null)
            {
                CurrentUnitOfWorkScope.Complete();
                CurrentUnitOfWorkScope.Dispose();
            }
        }

        protected IQueryable<T> Query<T>()
        {
            return CurrentUnitOfWork.Query<T>();
        }
    }
}