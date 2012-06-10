using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Taro.Commands;
using Taro.Data;
using Taro.TestUtils.Data;

namespace Taro.Tests.Commands
{
    public class AbstractCommandExecutorFacts
    {
        public class TheExecuteMethod
        {
            [Fact]
            public void will_bind_unit_of_work_before_execute_logic()
            {
                var unitOfWork = new MockUnitOfWork();
                UnitOfWorkFactory.Get = () => unitOfWork;

                var executor = new Command1Executor((uow, cmd) =>
                {
                    Assert.NotNull(ThreadStaticUnitOfWorkContext.Current);
                    Assert.Same(unitOfWork, ThreadStaticUnitOfWorkContext.Current);
                });

                executor.Execute(new Command1());
            }

            [Fact]
            public void will_unbind_unit_of_work_after_executed_logic_if_no_exception()
            {
                UnitOfWorkFactory.Get = () => new MockUnitOfWork();

                var executor = new Command1Executor();
                executor.Execute(new Command1());
                Assert.Null(ThreadStaticUnitOfWorkContext.Current);
            }

            [Fact]
            public void will_unbind_unit_of_work_after_executed_logic_even_if_exception_thrown()
            {
                UnitOfWorkFactory.Get = () => new MockUnitOfWork();

                var executor = new Command1Executor((uow, cmd) =>
                {
                    throw new InvalidOperationException();
                });

                try
                {
                    executor.Execute(new Command1());
                }
                catch { }

                Assert.Null(ThreadStaticUnitOfWorkContext.Current);
            }
        }

        public class Command1 : ICommand { }

        public class Command1Executor : ExecuteCommand<Command1>
        {
            public Action<IUnitOfWork, Command1> ExecuteAction { get; set; }

            public Command1Executor()
            {
            }

            public Command1Executor(Action<IUnitOfWork, Command1> executeAction)
            {
                ExecuteAction = executeAction;
            }

            protected override void Execute(IUnitOfWork unitOfWork, Command1 cmd)
            {
                if (ExecuteAction != null)
                {
                    ExecuteAction(unitOfWork, cmd);
                }
            }
        }
    }
}
