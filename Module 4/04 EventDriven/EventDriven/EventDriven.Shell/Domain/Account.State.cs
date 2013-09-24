using System;
using System.Collections.Generic;
using System.Linq;

using EventDriven.Shell.Domain.Events;
using EventDriven.Shell.Domain.Snapshots;

namespace EventDriven.Shell.Domain
{
    [Serializable]
    public partial class Account
    {
        Money balance = Money.Zero;      

        protected void When(AccountCredited e)
        {
            balance += Money.Amount(e.Amount);
        }

        protected void When(AccountDebited e)
        {
            balance =  Money.Amount(balance - Money.Amount(e.Amount));
        }

        protected override IMemento GetSnapshot()
        {
            return new AccountSnapshot
            {
                Balance = balance
            };
        }        

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (AccountSnapshot)memento;

            balance = snapshot.Balance;
        }
    }
}