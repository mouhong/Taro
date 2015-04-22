using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taro.Tryout.Domain.Events.Handlers
{
    class CustomerApprovedHandler : IHandles<CustomerApproved>
    {
        public void Handle(CustomerApproved theEvent)
        {
            using (var repo = AppRuntime.Instance.CreateDomainRepository<IRavenDomainRepository>())
            {
                var account = new CustomerAccount
                {
                    Id = "accounts/" + theEvent.CustomerId,
                    Balance = 100
                };

                repo.Save(account);
            }
        }
    }
}
