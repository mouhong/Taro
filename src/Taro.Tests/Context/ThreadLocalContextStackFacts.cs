using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Taro.Context;
using Taro.Events;
using Xunit;

namespace Taro.Tests.Context
{
    public class ThreadLocalContextStackFacts
    {
        public class TestContext
        {
            public string Id { get; set; }
        }

        [Fact]
        public void CanBindAndUnbind()
        {
            var context = new TestContext { Id = "#001" };

            Assert.Null(ThreadLocalContextStack<TestContext>.Current);

            ThreadLocalContextStack<TestContext>.Bind(context);
            Assert.NotNull(ThreadLocalContextStack<TestContext>.Current);

            ThreadLocalContextStack<TestContext>.Unbind();
            Assert.Null(ThreadLocalContextStack<TestContext>.Current);
        }

        [Fact]
        public void Nestable()
        {
            var ctx1 = new TestContext
            {
                Id = "#1"
            };

            ThreadLocalContextStack<TestContext>.Bind(ctx1);
            Assert.Equal("#1", ThreadLocalContextStack<TestContext>.Current.Id);

            var ctx2 = new TestContext
            {
                Id = "#2"
            };

            ThreadLocalContextStack<TestContext>.Bind(ctx2);
            Assert.Equal("#2", ThreadLocalContextStack<TestContext>.Current.Id);

            var ctx3 = new TestContext
            {
                Id = "#3"
            };

            ThreadLocalContextStack<TestContext>.Bind(ctx3);
            Assert.Equal("#3", ThreadLocalContextStack<TestContext>.Current.Id);

            ThreadLocalContextStack<TestContext>.Unbind();
            Assert.Equal("#2", ThreadLocalContextStack<TestContext>.Current.Id);

            ThreadLocalContextStack<TestContext>.Unbind();
            Assert.Equal("#1", ThreadLocalContextStack<TestContext>.Current.Id);

            ThreadLocalContextStack<TestContext>.Unbind();
            Assert.Null(ThreadLocalContextStack<TestContext>.Current);
        }

        [Fact]
        public void WillNotCrossThread()
        {
            ThreadLocalContextStack<TestContext>.Bind(new TestContext
            {
                Id = "#1"
            });

            var thread1 = new Thread(() =>
            {
                ThreadLocalContextStack<TestContext>.Bind(new TestContext
                {
                    Id = "#2"
                });
            });

            thread1.Start();
            thread1.Join();

            Assert.Equal("#1", ThreadLocalContextStack<TestContext>.Current.Id);

            thread1 = new Thread(() =>
            {
                Assert.Throws<InvalidOperationException>(() =>
                {
                    ThreadLocalContextStack<TestContext>.Unbind();
                });
            });

            thread1.Start();
            thread1.Join();

            Assert.Equal("#1", ThreadLocalContextStack<TestContext>.Current.Id);

            ThreadLocalContextStack<TestContext>.Unbind();
        }
    }
}
