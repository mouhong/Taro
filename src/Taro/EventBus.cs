using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Persistence;

namespace Taro
{
    public class EventBus : IEventBus
    {
        private ILocalTransactionContextFactory _transactionContextFactory;

        public EventBus(ILocalTransactionContextFactory transactionContextFactory)
        {
            _transactionContextFactory = transactionContextFactory;
        }

        public void Publish(IEvent @event)
        {
            using (var context = _transactionContextFactory.CreateLocalTransactionContext())
            {
                Publish(@event, context);
                context.Commit();
            }
        }

        public void Publish(IEvent @event, ILocalTransactionContext context)
        {
            context.AddEvents(new List<IEvent> { @event });
        }
    }
}
