using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taro.Dispatching;
using Xunit;

namespace Taro.Tests.Dispatching
{
    public class TypeUtilFacts
    {
        public class TheIsAttributeDefinedInMethodOrDeclaringClassMethod
        {
            [Fact]
            public void CanCheckMethodLevelAttributes()
            {
                var method = typeof(AttributeDefinedInMethodLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.True(TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute)));
            }

            [Fact]
            public void CanCheckClassLevelAttributes()
            {
                var method = typeof(AttributeDefinedInClassLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.True(TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(AwaitCommittedAttribute)));
            }

            [Fact]
            public void ReturnFalseIfAttributeNotDefined()
            {
                var method = typeof(AttributeDefinedInMethodLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.False(TypeUtil.IsAttributeDefinedInMethodOrDeclaringClass(method, typeof(HandleAsyncAttribute)));
            }

            public class AttributeDefinedInMethodLevel
            {
                [AwaitCommitted]
                public void Method() { }
            }

            [AwaitCommitted]
            public class AttributeDefinedInClassLevel
            {
                public void Method() { }
            }
        }

        public class TheGetOpenGenericArgumentTypesMethod
        {
            [Fact]
            public void ReturnEmptyCollectionIfNothingFound()
            {
                var eventTypes = TypeUtil.GetOpenGenericArgumentTypes(typeof(NonHandler), typeof(IHandle<>)).ToList();
                Assert.Equal(0, eventTypes.Count);
            }

            [Fact]
            public void WillReturnGenericArgumentTypes()
            {
                var eventTypes = TypeUtil.GetOpenGenericArgumentTypes(typeof(HandleSingleEvent), typeof(IHandle<>)).ToList();
                Assert.Equal(1, eventTypes.Count);
                Assert.Equal(typeof(Event2), eventTypes[0]);
            }

            [Fact]
            public void WillReturnAllGenericArgumentTypes()
            {
                var eventTypes = TypeUtil.GetOpenGenericArgumentTypes(typeof(HandleMultiEvents), typeof(IHandle<>)).ToList();
                Assert.Equal(2, eventTypes.Count);
                Assert.Equal(typeof(Event1), eventTypes[0]);
                Assert.Equal(typeof(Event3), eventTypes[1]);
            }

            public class Event1 : IEvent { }

            public class Event2 : IEvent { }

            public class Event3 : IEvent { }

            public class NonHandler { }

            public class HandleSingleEvent : IHandle<Event2>
            {
                public void Handle(Event2 evnt) { }
            }

            public class HandleMultiEvents : IHandle<Event1>, IHandle<Event3>
            {
                public void Handle(Event1 evnt) { }

                public void Handle(Event3 evnt) { }
            }
        }
    }
}
