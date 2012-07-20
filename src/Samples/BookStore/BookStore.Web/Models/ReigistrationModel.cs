using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Web.Models
{
    public class RegistrationModel
    {
        public string Email { get; set; }

        public string NickName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public Gender Gender { get; set; }

        public RegistrationModel()
        {
        }
    }
}
