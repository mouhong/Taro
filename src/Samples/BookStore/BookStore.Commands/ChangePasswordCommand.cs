using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro;

namespace BookStore.Commands
{
    public class ChangePasswordCommand
    {
        public string UserId { get; private set; }

        public string NewPassword { get; private set; }

        public ChangePasswordCommand(string userId, string newPassword)
        {
            UserId = userId;
            NewPassword = newPassword;
        }
    }
}
