using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Reflection;
using Taro.Utils;

namespace Taro.Commands.Buses
{
    public class DefaultCommandExecutorFinder : ICommandExecutorFinder
    {
        // Key: Command Type, Value: Executor Type
        private Dictionary<Type, Type> _executorTypes = new Dictionary<Type, Type>();

        public IExecuteCommand<TCommand> FindExecutor<TCommand>()
            where TCommand : ICommand
        {
            if (!_executorTypes.ContainsKey(typeof(TCommand)))
                throw new InvalidOperationException("Cannot find command executor. Command type: " + typeof(TCommand));

            return (IExecuteCommand<TCommand>)Activator.CreateInstance(_executorTypes[typeof(TCommand)]);
        }

        public bool RegisterExecutor(Type executorType)
        {
            Require.NotNull(executorType, "executorType");

            if (!executorType.IsClass || executorType.IsAbstract || executorType.IsGenericType) return false;

            foreach (var inter in executorType.GetInterfaces())
            {
                if (inter.IsGenericType && inter.GetGenericTypeDefinition() == typeof(IExecuteCommand<>))
                {
                    var commandType = inter.GetGenericArguments()[0];

                    if (_executorTypes.ContainsKey(commandType))
                        throw new InvalidOperationException("Cannot register duplicate command executor. Command type: " + commandType);

                    _executorTypes.Add(commandType, executorType);

                    return true;
                }
            }

            return false;
        }

        public void RegisterExecutors(params Assembly[] assemblies)
        {
            RegisterExecutors(assemblies as IEnumerable<Assembly>);
        }

        public void RegisterExecutors(IEnumerable<Assembly> assemblies)
        {
            Require.NotNull(assemblies, "assemblies");

            var executorInterfaceOpenGenericType = typeof(IExecuteCommand<>);

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    RegisterExecutor(type);
                }
            }
        }

        public void Clear()
        {
            _executorTypes.Clear();
        }
    }
}
