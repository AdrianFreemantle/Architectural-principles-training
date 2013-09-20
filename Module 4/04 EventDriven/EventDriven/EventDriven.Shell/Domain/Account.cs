using System.Linq;

namespace EventDriven.Shell.Domain
{
    public class AccountDebited : DomainEvent
    {
        public decimal Amount { get; protected set; } 

        public AccountDebited(decimal amount)
        {
            Amount = amount;
        }
    }

    public class AccountCredited : DomainEvent
    {
        public decimal Amount { get; protected set; }

        public AccountCredited(decimal amount)
        {
            Amount = amount;
        }
    }

    public class AccountHasInsufficientFundsForDebit : DomainEvent
    {
        public decimal DebitAmount { get; protected set; }
        public decimal CurrentBalance { get; protected set; }

        public AccountHasInsufficientFundsForDebit(decimal debitAmount, decimal currentBalance)
        {
            DebitAmount = debitAmount;
            CurrentBalance = currentBalance;
        }
    }

    public class Account : TypedEntity<AccountState>
    {
        public Account(Portfolio portfolio, AccountType accountType)
            : base(portfolio, new AccountId((int)accountType))
        {
        }

        public void Debit(Money amount)
        {
            var currentBalance = Balance();

            if ((currentBalance - amount) < 0)
            {
                RaiseEvent(new AccountHasInsufficientFundsForDebit(amount, currentBalance));
            }

            RaiseEvent(new AccountDebited(amount));
        }

        public void Credit(Money amount)
        {
            RaiseEvent(new AccountCredited(amount));
        }

        public decimal Balance()
        {
            return State.Transactions.Sum(transaction => transaction);
        }

        protected override IMemento GetSnapshot()
        {
            return ObjectCopier.Clone(State);
        }
    }
}