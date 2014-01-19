﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Events;

namespace Taro.Tests
{
    public class MockUnitOfWork : CommitableBase
    {
        public string Tag { get; set; }

        public Action CommitAction { get; set; }

        protected override void DoCommit()
        {
            if (CommitAction != null)
            {
                CommitAction();
            }
        }
    }
}
