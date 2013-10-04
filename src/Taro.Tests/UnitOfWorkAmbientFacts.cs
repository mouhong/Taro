using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Events;
using Xunit;

namespace Taro.Tests
{
    public class UnitOfWorkAmbientFacts
    {
        [Fact]
        public void CanBindAndUnbind()
        {
            var uow = new MockUnitOfWork();

            Assert.Null(UnitOfWorkAmbient.Current);

            UnitOfWorkAmbient.Bind(uow);
            Assert.NotNull(UnitOfWorkAmbient.Current);

            UnitOfWorkAmbient.Unbind();
            Assert.Null(UnitOfWorkAmbient.Current);
        }

        [Fact]
        public void Nestable()
        {
            var uow1 = new MockUnitOfWork
            {
                Tag = "#1"
            };

            UnitOfWorkAmbient.Bind(uow1);
            Assert.Equal("#1", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            var uow2 = new MockUnitOfWork
            {
                Tag = "#2"
            };

            UnitOfWorkAmbient.Bind(uow2);
            Assert.Equal("#2", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            var uow3 = new MockUnitOfWork
            {
                Tag = "#3"
            };

            UnitOfWorkAmbient.Bind(uow3);
            Assert.Equal("#3", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            UnitOfWorkAmbient.Unbind();
            Assert.Equal("#2", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            UnitOfWorkAmbient.Unbind();
            Assert.Equal("#1", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            UnitOfWorkAmbient.Unbind();
            Assert.Null(UnitOfWorkAmbient.Current);
        }

        [Fact]
        public void WillNotCrossThread()
        {
            UnitOfWorkAmbient.Bind(new MockUnitOfWork
            {
                Tag = "#1"
            });

            var thread1 = new Thread(() =>
            {
                UnitOfWorkAmbient.Bind(new MockUnitOfWork
                {
                    Tag = "#2"
                });
            });

            thread1.Start();
            thread1.Join();

            Assert.Equal("#1", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            thread1 = new Thread(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    UnitOfWorkAmbient.Unbind();
                });
            });

            thread1.Start();
            thread1.Join();

            Assert.Equal("#1", ((MockUnitOfWork)UnitOfWorkAmbient.Current).Tag);

            UnitOfWorkAmbient.Unbind();
        }
    }
}
