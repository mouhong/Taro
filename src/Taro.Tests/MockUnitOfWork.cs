using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events;

namespace Taro.Tests
{
    public class MockUnitOfWork : UnitOfWorkBase
    {
        public string Tag { get; set; }

        public Action CommitAction { get; set; }

        public new IList<IDomainEvent> UncommittedEvents
        {
            get
            {
                return base.UncommittedEvents;
            }
        }

        public MockUnitOfWork()
            : this(new DefaultEventDispatcher())
        {
        }

        public MockUnitOfWork(IEventDispatcher dispatcher)
            : base(dispatcher)
        {
        }

        protected override void DoCommit()
        {
            if (CommitAction != null)
            {
                CommitAction();
            }
        }
    }
}
