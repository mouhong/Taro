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
    public class RegistrationService
    {
        public ISession Session { get; private set; }

        public RegistrationService(ISession session)
        {
            Session = session;
        }

        public void Register(string email, string nickName, string password, Gender gender)
        {
            if (Session.Query<User>().ExistsEmail(email))
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
    }
}
