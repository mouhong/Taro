using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro;
using BookStore.Events;

namespace BookStore
{
    public class User
    {
        public virtual string Id { get; protected set; }

        public virtual string Email { get; protected set; }

        public virtual string Password { get; protected set; }

        public virtual string NickName { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual DateTime CreatedTime { get; protected set; }

        public virtual IList<BuyedBook> BuyedBooks { get; protected set; }

        public virtual Account Account { get; protected set; }

        protected User()
        {
        }

        public User(string email, Account account)
        {
            Id = Guid.NewGuid().ToString();
            Email = email;
            BuyedBooks = new List<BuyedBook>();
            CreatedTime = DateTime.Now;
            Account = account;
        }

        public virtual void SetInitialPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", "password");

            if (Password != null)
                throw new InvalidOperationException("Password has already been set.");

            Password = password;
        }

        public virtual void ChangePassword(string newPassword)
        {
            if (String.IsNullOrEmpty(newPassword))
                throw new ArgumentException("New password is required.", "newPassword");

            Password = newPassword;

            DomainEvent.Apply(new PasswordChangedEvent(Id, newPassword));
        }

        public virtual void BuyBook(Book book)
        {
            var now = DateTime.Now;

            Account.Decrease(book.Price, "Buy book: " + book.Title + "(ISBN: " + book.ISBN + ")");
            BuyedBooks.Add(new BuyedBook(this, book));

            DomainEvent.Apply(new BookBuyedEvent(Id, book.ISBN, book.Title, now, book.Price));
        }
    }
}
