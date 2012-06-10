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
    class ChangePasswordCommandExecutor : Executes<ChangePasswordCommand>
    {
        protected override void Execute(UnitOfWork unitOfWork, ChangePasswordCommand cmd)
        {
            var user = unitOfWork.Get<User>(cmd.UserId);
            user.ChangePassword(cmd.NewPassword);

            unitOfWork.Commit();
        }
    }
}
