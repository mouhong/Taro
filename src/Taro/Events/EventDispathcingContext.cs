using System;

namespace Taro.Events
{
    public class EventDispathcingContext
    {
        public IUnitOfWork UnitOfWork { get; private set; }

        public bool WasUnitOfWorkCommitted { get; private set; }

        public EventDispathcingContext(IUnitOfWork unitOfWork, bool wasUnitOfWorkCommitted)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            UnitOfWork = unitOfWork;
            WasUnitOfWorkCommitted = wasUnitOfWorkCommitted;
        }
    }
}
