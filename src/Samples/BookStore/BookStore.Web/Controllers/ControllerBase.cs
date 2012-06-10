using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NHibernate;
using NHibernate.Linq;

using Taro;
using Taro.Data;
using Taro.Commands;
using Taro.Commands.Buses;

namespace BookStore.Web.Controllers
{
    public class ControllerBase : Controller
    {
        protected Func<ISession> GetNhSession { get; private set; }

        private ISession _currentSession;

        protected ISession NhSession
        {
            get
            {
                if (_currentSession == null)
                {
                    _currentSession = GetNhSession();
                }
                return _currentSession;
            }
        }

        protected ICommandBus CommandBus { get; private set; }

        protected ControllerBase()
            : this(TaroEnvironment.Instance.CommandBus, () => SessionManager.Current.OpenSession())
        {
        }

        protected ControllerBase(ICommandBus commandBus, Func<ISession> getNhSession)
        {
            CommandBus = commandBus;
            GetNhSession = getNhSession;
        }

        protected IQueryable<T> Query<T>()
        {
            return NhSession.Query<T>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _currentSession != null)
            {
                _currentSession.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}