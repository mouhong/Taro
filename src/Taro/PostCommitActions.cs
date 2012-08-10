using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Utils;
using Taro.Data;
using System.Threading;

namespace Taro
{
    public static class PostCommitActions
    {
        static ThreadLocal<List<Action>> _actions = new ThreadLocal<List<Action>>(() => new List<Action>());

        public static void Enqueue(Action action)
        {
            Require.NotNull(action, "action");

            if (ThreadStaticUnitOfWorkContext.Current == null)
                throw new InvalidOperationException("Cannot enqueue post commit actions outside a UnitOfWorkScope. Ensure this code is wrapped in a UnitOfWorkScope.");

            _actions.Value.Add(action);
        }

        public static int Count()
        {
            return _actions.Value.Count;
        }

        public static IEnumerable<Action> GetQueuedActions()
        {
            return _actions.Value;
        }

        public static void Clear()
        {
            _actions.Value.Clear();
        }
    }
}
