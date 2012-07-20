using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Taro.Data;
using Taro.Events;

using BookStore.Services;

namespace BookStore.Events.Handlers
{
    class OnNewUserRegistered_SendEmail : IHandleEventOnCommit<NewUserRegisteredEvent>
    {
        public void Handle(NewUserRegisteredEvent evnt)
        {
            Debug.WriteLine("Send email to " + evnt.UserEmail + " to notify his registration.");
        }
    }

    class OnNewUserRegistered_UpdateStatistics : HandlesImmediately<NewUserRegisteredEvent>
    {
        public override void Handle(NewUserRegisteredEvent evnt)
        {
            var service = new WebsiteInfoService(UnitOfWork.Session);
            var totalUsers = Convert.ToInt32(service.GetValue(WebsiteInfoKeys.TotalUsers, "0"));
            service.SetValue(WebsiteInfoKeys.TotalUsers, (totalUsers + 1).ToString());
        }
    }
}
