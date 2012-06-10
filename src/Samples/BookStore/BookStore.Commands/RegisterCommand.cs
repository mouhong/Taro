using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;
using BookStore.Domain;

namespace BookStore.Commands
{
    public class RegisterCommand : ICommand
    {
        public string Email { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Gender Gender { get; set; }

        public RegisterCommand()
        {
        }
    }
}
