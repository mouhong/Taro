using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;
using Taro.Data;
using Taro.Commands;

using BookStore.Domain;
using BookStore.Domain.Services;

namespace BookStore.Commands.Executors
{
    class AddBookCommandExecutor : Executes<AddBookCommand>
    {
        protected override void Execute(UnitOfWork unitOfWork, AddBookCommand cmd)
        {
            if (String.IsNullOrEmpty(cmd.ISBN))
                throw new ArgumentException("ISBN is required.");

            if (cmd.Price <= 0)
                throw new ArgumentException("Price should be greater than 0.");

            if (String.IsNullOrEmpty(cmd.CreatorId))
                throw new ArgumentException("Creator ID is required.");

            var validator = new ISBNValidator(unitOfWork.Session);

            if (validator.ExistsISBN(cmd.ISBN))
                throw new InvalidOperationException("A book with the same ISBN already exists. You should edit the existing book.");

            var book = Book.Create(cmd.ISBN, cmd.Title, cmd.Author, cmd.PublishedDate, cmd.Price, cmd.Stock, cmd.CreatorId);
            unitOfWork.Save(book);

            unitOfWork.Commit();
        }
    }
}
