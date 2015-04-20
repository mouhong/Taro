using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;

namespace Taro
{
    public class EventBus : IEventBus
    {
        private ILocalTransactionContextFactory _localTransactionContextFactory;

        public EventBus(ILocalTransactionContextFactory localTransactionContextFactory)
        {
            Require.NotNull(localTransactionContextFactory, "localTransactionContextFactory");
            _localTransactionContextFactory = localTransactionContextFactory;
        }

        public void Publish(IEvent theEvent)
        {
            Require.NotNull(theEvent, "theEvent");

            using (var context = _localTransactionContextFactory.CreateLocalTransactionContext())
            {
                Publish(theEvent, context);
                context.Commit();
            }
        }

        public void Publish(IEvent theEvent, ILocalTransactionContext context)
        {
            Require.NotNull(theEvent, "theEvent");
            Require.NotNull(context, "context");

            context.AddEvents(new List<IEvent> { theEvent });
        }
    }
}
