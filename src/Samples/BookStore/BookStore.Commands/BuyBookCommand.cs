using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;

namespace BookStore.Commands
{
    public class BuyBookCommand : ICommand
    {
        public string UserEmail { get; private set; }

        public string BookISBN { get; private set; }

        public BuyBookCommand(string userEmail, string bookISBN)
        {
            UserEmail = userEmail;
            BookISBN = bookISBN;
        }
    }
}
