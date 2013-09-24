using EventDriven.Shell.Domain.Events;

namespace EventDriven.Shell.Domain
{
    public partial class Account : Entity
    {
        protected Account(Portfolio portfolio, AccountId identity)
            : base(portfolio, identity)
        {
        }

        public static Account OpenAccount(Portfolio portfolio, AccountType accountType)
        {
            var account = new Account(portfolio, new AccountId(accountType));            
            return account; 
        }

        public void Debit(Money amount)
        {
            if (balance - amount < 0)
            {
                RaiseEvent(new AccountHadInsufficientFundsForDebit(amount, balance));
                return;
            }

            RaiseEvent(new AccountDebited(amount));
        }

        public void Credit(Money amount)
        {
            RaiseEvent(new AccountCredited(amount));
        }

        public Money CurrentBalance()
        {
            return balance;
        }
    }
}