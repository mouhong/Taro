using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Taro.Data;
using Taro.Events;

namespace BookStore.Events.Handlers
{
    class OnBalanceChanged : HandlesImmediately<BalanceChangedEvent>
    {
        public override void Handle(BalanceChangedEvent evnt)
        {
            var account = UnitOfWork.Get<Account>(evnt.AccountId);
            var notificationSettings = account.NotificationSettings;

            if (notificationSettings.Enabled && notificationSettings.MinAmount < Math.Abs(evnt.Amount))
            {
                var message = String.Format("Account {0} -> Balance {1}: {2}.", evnt.AccountId, evnt.Amount > 0 ? "increased" : "decreased", Math.Abs(evnt.Amount));
                Debug.WriteLine(message);
            }
        }
    }
}
