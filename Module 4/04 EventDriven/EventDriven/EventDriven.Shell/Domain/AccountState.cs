using System;
using System.Collections.Generic;

namespace EventDriven.Shell.Domain
{
    [Serializable]
    public class AccountState : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public List<Transaction> Transactions { get; set; }

        public AccountState()
        {
            Transactions = new List<Transaction>();
        }

        public void When(AccountCredited e)
        {
            Transactions.Add(new Transaction(Money.Amount(e.Amount), TransactionType.Credit));
        }

        public void When(AccountDebited e)
        {
            Transactions.Add(new Transaction(Money.Amount(e.Amount), TransactionType.Debit));
        }
    }
}