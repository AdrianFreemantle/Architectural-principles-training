﻿using System.Collections.Generic;
using System.Linq;

using EventDriven.Shell.Domain.Events;
using EventDriven.Shell.Domain.Snapshots;

namespace EventDriven.Shell.Domain
{
    public partial class Portfolio
    {
        private bool isOpen;
        private readonly HashSet<Account> accounts;

        protected void When(PortfolioOpened e)
        {
            isOpen = true;
        }

        protected void When(AccountOpened e)
        {
            accounts.Add(Account.OpenAccount(this, e.AccountType));
        }

        protected override IMemento GetSnapshot()
        {
            return new PortfolioMemento
            {
                IsOpen = isOpen,
                Accounts = accounts.ToArray()
                                   .Select(account => ((IAmRestorable)account).GetSnapshot())
                                   .Cast<AccountSnapshot>()
                                   .ToArray()
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var portfolioMemento = (PortfolioMemento)memento;

            isOpen = portfolioMemento.IsOpen;

            foreach (var accountSnapshot in portfolioMemento.Accounts)
            {
                accounts.Add(RestoreEntity<Account>(accountSnapshot));
            }
        }
    }
}