using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NHibernate;
using NHibernate.Linq;

using Taro;

using BookStore.Domain.Events;

namespace BookStore.Domain.Services
{
    public class MessageService
    {
        private ISession _session;

        public MessageService(ISession session)
        {
            _session = session;
        }

        public int GetTotalUnReadMessageCount(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("'userId' is required.");

            var box = _session.Get<MessageBoxInfo>(userId);
            return box == null ? 0 : box.UnReadMessages;
        }

        public int GetTotalMessageCount(string userId)
        {
            if (String.IsNullOrEmpty(userId))
                throw new ArgumentException("'userId' is required.");

            var box = _session.Get<MessageBoxInfo>(userId);
            return box == null ? 0 : box.TotalMessages;
        }

        public void Send(string receiverId, string content)
        {
            if (String.IsNullOrEmpty(receiverId))
                throw new ArgumentException("'receiverId' is required.");

            var msg = new Message(receiverId);
            msg.Content = content;
            msg.SentTime = DateTime.Now;

            _session.Save(msg);

            DomainEvent.Apply(new MessageSentEvent(receiverId, content));
        }
    }
}
