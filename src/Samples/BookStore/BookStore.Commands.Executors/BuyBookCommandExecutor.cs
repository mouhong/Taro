using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;
using Taro.Data;
using Taro.Commands;

using BookStore.Domain;

namespace BookStore.Commands.Executors
{
    class BuyBookCommandExecutor : Executes<BuyBookCommand>
    {
        protected override void Execute(UnitOfWork unitOfWork, BuyBookCommand cmd)
        {
            var user = unitOfWork.Query<User>().First(it => it.Email == cmd.UserEmail);
            var book = unitOfWork.Get<Book>(cmd.BookISBN);

            user.BuyBook(book);

            unitOfWork.Commit();
        }
    }
}
