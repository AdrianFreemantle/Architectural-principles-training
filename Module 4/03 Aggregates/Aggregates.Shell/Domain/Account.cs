using System;
using System.Collections.Generic;
using System.Linq;
using Aggregates.Shell.Domain.Snapshots;

namespace Aggregates.Shell.Domain
{
    public class Account : Entity
    {
        private readonly List<Transaction> transactions;

        public Account(Portfolio portfolio, AccountId accountId)
            : base(portfolio, accountId)
        {
            transactions = new List<Transaction>();
        }

        public void Debit(Money amount)
        {
            var currentBalance = Balance();

            if ((currentBalance - amount) < 0)
            {
                throw new InvalidOperationException(String.Format("Unable to withdraw {0} as current balance is {1}", amount, currentBalance));
            }

            transactions.Add(new Transaction(amount, TransactionType.Debit));
        }

        public void Credit(Money amount)
        {
            transactions.Add(new Transaction(amount, TransactionType.Credit));
        }

        public Money Balance()
        {
            return Money.Amount(transactions.Sum(transaction => transaction));
        }

        protected override IMemento GetSnapshot()
        {
            return new AccountSnapshot
            {
                Transactions = transactions.ToArray()
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (AccountSnapshot)memento;

            transactions.AddRange(snapshot.Transactions);
        }
    }
}