using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro
{
    /// <summary>
    /// A simple object container to manage Taro objects.
    /// This is not intended to be used as a container for user objects.
    /// Taro objects are supposed to be registered on app startup, so we don't make this class thread-safe.
    /// </summary>
    public class ObjectContainer
    {
        private readonly Dictionary<Type, IRegisterationEntry> _entries = new Dictionary<Type,IRegisterationEntry>();

        public bool HasRegistrationFor(Type type)
        {
            Require.NotNull(type, "type");
            return _entries.ContainsKey(type);
        }

        public bool HasRegistrationFor<T>()
        {
            return HasRegistrationFor(typeof(T));
        }

        public T Resolve<T>() where T : class
        {
            IRegisterationEntry entry;
            if (_entries.TryGetValue(typeof(T), out entry))
            {
                return (T)entry.Resolve(this);
            }

            return null;
        }

        public void Register<T>(T instance) where T : class
        {
            Require.NotNull(instance, "instance");

            var entry = new InstanceRegistrationEntry<T>(instance);
            var type = typeof(T);
            if (_entries.ContainsKey(type))
            {
                _entries[type] = entry;
            }
            else
            {
                _entries.Add(type, entry);
            }
        }

        public void Register<T>(Func<ObjectContainer, T> factory)
        {
            Require.NotNull(factory, "factory");

            var entry = new FactoryRegistrationEntry<T>(factory);
            var type = typeof(T);
            if (_entries.ContainsKey(type))
            {
                _entries[type] = entry;
            }
            else
            {
                _entries.Add(type, entry);
            }
        }

        interface IRegisterationEntry
        {
            object Resolve(ObjectContainer container);
        }

        class InstanceRegistrationEntry<T> : IRegisterationEntry
        {
            private T _instance;

            public InstanceRegistrationEntry(T instance)
            {
                _instance = instance;
            }

            public object Resolve(ObjectContainer container)
            {
                return _instance;
            }
        }

        class FactoryRegistrationEntry<T> : IRegisterationEntry
        {
            private Func<ObjectContainer, T> _factory;

            public FactoryRegistrationEntry(Func<ObjectContainer, T> factory)
            {
                _factory = factory;
            }

            public object Resolve(ObjectContainer container)
            {
                return _factory(container);
            }
        }
    }
}
