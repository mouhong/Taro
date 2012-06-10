using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Taro;
using Taro.Data;
using Taro.Commands;

using BookStore.Domain.Services;

namespace BookStore.Commands.Executors
{
    class RegisterCommandExecutor : Executes<RegisterCommand>
    {
        protected override void Execute(UnitOfWork unitOfWork, RegisterCommand cmd)
        {
            if (cmd.Password != cmd.ConfirmPassword)
                throw new InvalidOperationException("Password not match.");

            var service = new UserService(unitOfWork.Session);
            service.Register(cmd.Email, cmd.NickName, cmd.Password, cmd.Gender);

            unitOfWork.Commit();
        }
    }
}
