using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Taro.Data;
using Taro.TestUtils.Data;

namespace Taro.Tests.Data
{
    public class UnitOfWorkScopeFacts
    {
        public class TheConstructor
        {
            [Fact]
            public void will_bind_unit_of_work_context()
            {
                var unitOfWork = new MockUnitOfWork();
                using (var scope = new UnitOfWorkScope(unitOfWork))
                {
                    Assert.NotNull(ThreadStaticUnitOfWorkContext.Current);
                    Assert.Same(unitOfWork, ThreadStaticUnitOfWorkContext.Current);
                }
            }
        }

        public class TheDisposeMethod
        {
            [Fact]
            public void will_unbind_unit_of_work_context()
            {
                using (var scope = new UnitOfWorkScope(new MockUnitOfWork())) { }

                Assert.Null(ThreadStaticUnitOfWorkContext.Current);
            }

            [Fact]
            public void will_dispose_unit_of_work()
            {
                var unitOfWork = new MockUnitOfWork();
                using (var scope = new UnitOfWorkScope(unitOfWork)) { }

                Assert.True(unitOfWork.IsDisposed);
            }
        }
    }
}
