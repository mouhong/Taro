using System;

namespace Taro
{
    public static class UnitOfWorkAmbient
    {
        [ThreadStatic]
        private static IUnitOfWork _current;

        public static IUnitOfWork Current
        {
            get
            {
                return _current;
            }
        }

        public static void Bind(IUnitOfWork unitOfWork)
        {
            Require.NotNull(unitOfWork, "unitOfWork");
            _current = unitOfWork;
        }

        public static void Unbind()
        {
            _current = null;
        }
    }
}
