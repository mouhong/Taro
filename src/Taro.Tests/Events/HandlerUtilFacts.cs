using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Taro.Events;
using Xunit;

namespace Taro.Tests.Events
{
    public class HandlerUtilFacts
    {
        public class TheIsAttributeDefinedMethod
        {
            [Fact]
            public void CanCheckMethodLevelAttributes()
            {
                var method = typeof(AttributeDefinedInMethodLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.True(HandlerUtil.IsAttributeDefined(method, typeof(AwaitCommittedAttribute)));
            }

            [Fact]
            public void CanCheckClassLevelAttributes()
            {
                var method = typeof(AttributeDefinedInClassLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.True(HandlerUtil.IsAttributeDefined(method, typeof(AwaitCommittedAttribute)));
            }

            [Fact]
            public void ReturnFalseIfAttributeNotDefined()
            {
                var method = typeof(AttributeDefinedInMethodLevel).GetMethod("Method", BindingFlags.Public | BindingFlags.Instance);
                Assert.False(HandlerUtil.IsAttributeDefined(method, typeof(HandleAsyncAttribute)));
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

        public class TheGetHandledEventTypesMethod
        {
            [Fact]
            public void ReturnEmptyCollectionForNonHandlerType()
            {
                var eventTypes = HandlerUtil.GetHandledEventTypes(typeof(NonHandler)).ToList();
                Assert.Equal(0, eventTypes.Count);
            }

            [Fact]
            public void CanReturnSingleHandledEvent()
            {
                var eventTypes = HandlerUtil.GetHandledEventTypes(typeof(HandleSingleEvent)).ToList();
                Assert.Equal(1, eventTypes.Count);
                Assert.Equal(typeof(Event2), eventTypes[0]);
            }

            [Fact]
            public void CanReturnMultiHandledEvents()
            {
                var eventTypes = HandlerUtil.GetHandledEventTypes(typeof(HandleMultiEvents)).ToList();
                Assert.Equal(2, eventTypes.Count);
                Assert.Equal(typeof(Event1), eventTypes[0]);
                Assert.Equal(typeof(Event3), eventTypes[1]);
            }

            public class Event1 : DomainEvent{}

            public class Event2 : DomainEvent{}

            public class Event3 : DomainEvent{}

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
