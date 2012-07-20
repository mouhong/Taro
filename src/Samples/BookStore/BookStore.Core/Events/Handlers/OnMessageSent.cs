using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using Taro.Events;

namespace BookStore.Events.Handlers
{
    public class OnMessageSent : HandlesImmediately<MessageSentEvent>
    {
        public override void Handle(MessageSentEvent evnt)
        {
            var box = UnitOfWork.Get<MessageBoxInfo>(evnt.ReceiverId);

            if (box == null)
            {
                box = new MessageBoxInfo(evnt.ReceiverId);
                UnitOfWork.Save(box);
            }

            box.TotalMessages++;
            box.UnReadMessages++;
        }
    }
}
