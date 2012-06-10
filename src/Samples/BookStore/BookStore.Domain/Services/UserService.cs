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
    public class UserService
    {
        public ISession Session { get; private set; }

        public UserService(ISession session)
        {
            Session = session;
        }

        public User GetUserByEmail(string email)
        {
            return Session.Query<User>().FirstOrDefault(it => it.Email == email);
        }

        public string GetUserIdByEmail(string email)
        {
            return Session.Query<User>().Where(it => it.Email == email).Select(it => it.Id).FirstOrDefault();
        }

        public bool Authenticate(string email, string password)
        {
            return Session.Query<User>().Any(it => it.Email == email && it.Password == password);
        }

        public void Register(string email, string nickName, string password, Gender gender)
        {
            if (IsEmailUsed(email))
                throw new InvalidOperationException("Registration failed. Email was used.");

            var account = new Account();

            var user = new User(email, account)
            {
                NickName = nickName,
                Gender = gender
            };

            user.SetInitialPassword(password);

            Session.Save(account);
            Session.Save(user);

            DomainEvent.Apply(new NewUserRegisteredEvent
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserNickName = user.NickName
            });
        }

        public bool IsEmailUsed(string email)
        {
            return Session.Query<User>().Any(it => it.Email == email);
        }
    }
}
