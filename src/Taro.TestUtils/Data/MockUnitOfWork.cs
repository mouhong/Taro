using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Data;
using Taro.TestUtils.Events.Buses;

namespace Taro.TestUtils.Data
{
    public class MockUnitOfWork : AbstractUnitOfWork
    {
        public bool IsDisposed { get; private set; }

        public MockUnitOfWork()
            : base(new MockEventBus())
        {
        }

        protected override void DoCommit()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            IsDisposed = true;
        }
    }
}
