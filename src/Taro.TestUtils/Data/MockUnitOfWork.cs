using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Data;
using Taro.Events.Storage;
using Taro.TestUtils.Events.Buses;

namespace Taro.TestUtils.Data
{
    public class MockUnitOfWork : AbstractUnitOfWork
    {
        public MockUnitOfWork()
            : base(new MockEventBus(), new NullEventStore())
        {
        }

        protected override void CommitChanges()
        {
        }
    }
}
