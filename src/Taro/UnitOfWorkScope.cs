using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Config;
using Taro.Context;
using Taro.Events;

namespace Taro
{
    public class UnitOfWorkScope : IDisposable
    {
        private IEventDispatcher _dispatcher;
        private ICommitable _unitOfWork;
        private List<IEvent> _events;

        public ICommitable UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        public UnitOfWorkScope(ICommitable unitOfWork, IEventDispatcher dispatcher)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            Require.NotNull(dispatcher, "dispatcher");

            _unitOfWork = unitOfWork;
            _dispatcher = dispatcher;
            _events = new List<IEvent>();

            _unitOfWork.Comitted += OnUnitOfWorkComitted;
            Event.RegisterEventAppliedCallback(OnEventApplied);
            UnitOfWorkScopeContext.Bind(this);
        }

        void OnEventApplied(IEvent evnt)
        {
            _events.Add(evnt);
        }

        void OnUnitOfWorkComitted(object sender, EventArgs e)
        {
            var events = _events.ToList();
            _events.Clear();

            var context = new EventDispatchingContext(EventDispatchingPhase.OnUnitOfWorkCommitted, this);

            foreach (var evnt in events)
            {
                _dispatcher.Dispatch(evnt, context);
            }
        }

        public static UnitOfWorkScope Begin(ICommitable unitOfWork)
        {
            return Begin(unitOfWork, Taro.Config.TaroEnvironment.Instance.EventDispatcher);
        }

        public static UnitOfWorkScope Begin(ICommitable unitOfWork, IEventDispatcher dispatcher)
        {
            return new UnitOfWorkScope(unitOfWork, dispatcher);
        }

        public void Dispose()
        {
            _unitOfWork.Comitted -= OnUnitOfWorkComitted;
            Event.UnregisterEventAppliedCallback(OnEventApplied);
            UnitOfWorkScopeContext.Unbind();
        }
    }
}
