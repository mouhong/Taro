using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Web
{
    public class UserUtil
    {
        public static bool IsAdmin(string email)
        {
            return email == "admin@admin.com";
        }
    }
}