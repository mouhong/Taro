using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;
using Taro.Data;
using Taro.TestUtils.Data;

namespace Taro.Tests.Data
{
    public class ThreadStaticUnitOfWorkContextFacts
    {
        [Fact]
        public void different_thread_will_have_different_IUnitOfWork()
        {
            var mainThreadUnitOfWork = new MockUnitOfWork();

            ThreadStaticUnitOfWorkContext.Bind(mainThreadUnitOfWork);

            var childThread = new Thread(() =>
            {
                var childThreadUnitOfWork = new MockUnitOfWork();
                ThreadStaticUnitOfWorkContext.Bind(childThreadUnitOfWork);

                Assert.Same(childThreadUnitOfWork, ThreadStaticUnitOfWorkContext.Current);
            });

            childThread.Start();
            childThread.Join();

            Assert.Same(mainThreadUnitOfWork, ThreadStaticUnitOfWorkContext.Current);
        }

        [Fact]
        public void Unbind_will_set_Current_property_to_null()
        {
            ThreadStaticUnitOfWorkContext.Bind(new MockUnitOfWork());
            ThreadStaticUnitOfWorkContext.Unbind();

            Assert.Null(ThreadStaticUnitOfWorkContext.Current);
        }
    }
}
