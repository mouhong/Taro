using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taro.Samples.Data;
using Taro.Samples.Domain.Events;

namespace Taro.Samples.Domain
{
    public class MoneyTransferService
    {
        private UnitOfWork _unitOfWork;

        public MoneyTransferService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Transfer(BankAccount sourceAccount, BankAccount destinationAccount, decimal amount)
        {
            if (sourceAccount == null)
                throw new ArgumentNullException("sourceAccount");

            if (destinationAccount == null)
                throw new ArgumentNullException("destinationAccount");

            sourceAccount.Decrease(amount);
            destinationAccount.Increase(amount);

            DomainEvent.Apply(new MoneyTransferCompleted(sourceAccount, destinationAccount, amount));
        }
    }
}
