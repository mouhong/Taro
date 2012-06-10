using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro.Data;
using Taro.Commands;
using BookStore.Domain;
using BookStore.Domain.Services;

namespace BookStore.Commands.Executors
{
    class MarkAllMessagesAsReadCommandExecutor : Executes<MarkAllMessagesAsReadCommand>
    {
        protected override void Execute(UnitOfWork unitOfWork, MarkAllMessagesAsReadCommand cmd)
        {
            var userId = new UserService(unitOfWork.Session).GetUserIdByEmail(cmd.UserEmail);
            var messages = unitOfWork.Query<Message>()
                                     .Where(it => !it.IsRead && it.UserId == userId);

            foreach (var each in messages)
            {
                each.IsRead = true;
            }

            var box = unitOfWork.Get<MessageBoxInfo>(userId);
            if (box != null)
            {
                box.UnReadMessages = 0;
            }

            unitOfWork.Commit();
        }
    }
}
