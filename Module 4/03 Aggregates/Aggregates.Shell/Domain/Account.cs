using System;
using System.Linq;

namespace Aggregates.Shell.Domain
{
    public class Account : TypedEntity<AccountState>
    {
        public Account(AccountType accountType)
            : base(new AccountId((int)accountType))
        {
        }

        public void Debit(Money amount)
        {
            var currentBalance = Balance();

            if ((currentBalance - amount) < 0)
            {
                throw new InvalidOperationException(String.Format("Unable to withdraw {0} as current balance is {1}", amount, currentBalance));
            }

            State.Transactions.Add(new Transaction(amount, TransactionType.Debit));
        }

        public void Credit(Money amount)
        {
            State.Transactions.Add(new Transaction(amount, TransactionType.Credit));
        }

        public Money Balance()
        {
            return Money.Amount(State.Transactions.Sum(transaction => transaction));
        }

        protected override IMemento GetSnapshot()
        {
            return ObjectCopier.Clone(State);
        }
    }
}