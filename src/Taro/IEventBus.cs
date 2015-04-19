using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taro.Persistence;

namespace Taro
{
    public interface IEventBus
    {
        void Publish(IEvent @event);

        void Publish(IEvent @event, ILocalTransactionContext localTransactionContext);
    }
}
