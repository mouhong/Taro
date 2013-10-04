using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Taro.Tests
{
    public class UnitOfWorkScopeFacts
    {
        public class TheParameterlessConstructor
        {
            [Fact]
            public void WillBindUnitOfWorkAmbient()
            {
                var uow = new MockUnitOfWork();

                Taro.Config.TaroEnvironment.Configure(x =>
                {
                    x.UsingUnitOfWorkFactory(() => uow);
                });

                var scope = new UnitOfWorkScope<MockUnitOfWork>();

                Assert.NotNull(UnitOfWorkAmbient.Current);
                Assert.Same(uow, UnitOfWorkAmbient.Current);

                scope.Dispose();
            }
        }

        public class TheNonParameterlessConstructor
        {
            [Fact]
            public void WillBindUnitOfWorkAmbinet()
            {
                var uow = new MockUnitOfWork();
                var scope = new UnitOfWorkScope<MockUnitOfWork>(uow);

                Assert.NotNull(UnitOfWorkAmbient.Current);
                Assert.Same(uow, UnitOfWorkAmbient.Current);

                scope.Dispose();
            }
        }

        public class TheCompleteMethod
        {
            [Fact]
            public void WillCommitUnitOfWork()
            {
                var totalCommit = 0;
                var uow = new MockUnitOfWork();
                uow.CommitAction = () =>
                {
                    totalCommit++;
                };

                var scope = new UnitOfWorkScope<MockUnitOfWork>(uow);
                scope.Complete();

                Assert.Equal(1, totalCommit);

                scope.Dispose();
            }
        }

        public class TheDisposeMethod
        {
            [Fact]
            public void WillUnbindUnitOfWorkAmbinet()
            {
                var scope = new UnitOfWorkScope<MockUnitOfWork>(new MockUnitOfWork());
                scope.Dispose();
                Assert.Null(UnitOfWorkAmbient.Current);
            }
        }
    }
}
