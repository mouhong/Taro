using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Domain
{
    public static class UserQueryableExtensions
    {
        public static User Find(this IQueryable<User> users, string email)
        {
            return users.FirstOrDefault(it => it.Email == email);
        }

        public static bool ExistsEmail(this IQueryable<User> users, string email)
        {
            return users.Any(it => it.Email == email);
        }
    }
}
