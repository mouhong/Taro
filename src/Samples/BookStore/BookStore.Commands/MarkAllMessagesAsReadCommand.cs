using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;

namespace BookStore.Commands
{
    public class MarkAllMessagesAsReadCommand : ICommand
    {
        public string UserEmail { get; private set; }

        public MarkAllMessagesAsReadCommand(string email)
        {
            UserEmail = email;
        }
    }
}
