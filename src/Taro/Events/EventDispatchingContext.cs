using System;

namespace Taro.Events
{
    public class EventDispatchingContext
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public bool WasUnitOfWorkCommitted { get; private set; }

        public EventDispatchingContext(IUnitOfWork unitOfWork, bool wasUnitOfWorkCommitted)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            UnitOfWork = unitOfWork;
            WasUnitOfWorkCommitted = wasUnitOfWorkCommitted;
        }
    }
}
