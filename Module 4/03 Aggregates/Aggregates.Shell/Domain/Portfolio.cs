using System;
using System.Collections.Generic;
using System.Linq;
using Aggregates.Shell.Domain.Snapshots;

namespace Aggregates.Shell.Domain
{
    public class Portfolio : Aggregate
    {
        private bool isOpen;
        private readonly HashSet<Account> accounts;

        private static readonly Money MinimumPortfolioBalance = Money.Amount(100);

        protected Portfolio(PortfolioId id) 
            : base(id)
        {
            accounts = new HashSet<Account>();
            isOpen = true;
        }

        public static Portfolio Open(PortfolioId id, AccountType accountType, Money initialDeposit)
        {
            if (initialDeposit < MinimumPortfolioBalance)
            {
                throw new InvalidOperationException(String.Format("The intial deposit of {0} is lower than the require a minimum of {1}", initialDeposit, MinimumPortfolioBalance));
            }

            var portfolio = new Portfolio(id);
            portfolio.OpenAccount(accountType);
            portfolio.CreditAccount(accountType, initialDeposit);

            return portfolio;
        }

        public void ClosePortfolio()
        {
            isOpen = false;
        }

        public void OpenAccount(AccountType accountType)
        {
            GuardPortfolioState();
            GuardAccountType(accountType);
            accounts.Add(new Account(this, new AccountId(accountType)));
        }

        public void DebitAccount(AccountType accountType, Money amount)
        {
            GuardPortfolioState();
            GuardPortfolioBalance(accountType, amount);

            var account = Get<Account>(new AccountId(accountType));
            account.Debit(amount);
        }

        public void CreditAccount(AccountType accountType, Money amount)
        {
            GuardPortfolioState();

            var account = Get<Account>(new AccountId(accountType));
            account.Credit(amount);
        }

        public Money GetAccountBalance(AccountType accountType)
        {
            var account = Get<Account>(new AccountId(accountType));
            return account.Balance();
        }

        public Money GetPortfolioBalance()
        {
            Money balance = Money.Zero;

            foreach (var account in accounts)
            {
                balance += account.Balance();
            }

            return balance;
        }

        private void GuardPortfolioBalance(AccountType accountType, Money debitAmout)
        {
            var currentBalance = GetPortfolioBalance();

            if ((currentBalance - debitAmout) < MinimumPortfolioBalance)
            {
                throw new InvalidOperationException(String.Format("Unable to withdraw {0} from account {1} on portfolio {2} as it would exceed the portfolio balance limit of {3}",  
                    debitAmout, accountType, Identity.GetId(), MinimumPortfolioBalance));
            }
        }

        private void GuardPortfolioState()
        {
            if (!isOpen)
            {
                throw new InvalidOperationException("Account is closed");
            }
        }

        private void GuardAccountType(AccountType accountType)
        {
            if (accountType == AccountType.Unknown)
            {
                throw new InvalidOperationException("You must provide a valid account type.");
            }
        }

        protected override IMemento GetSnapshot()
        {
            return new PortfolioSnapshot
            {
                IsOpen = isOpen,
                Accounts = accounts.Select(account => ((IEntity)account).GetSnapshot()).ToArray()
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (PortfolioSnapshot)memento;

            isOpen = snapshot.IsOpen;

            foreach (var account in snapshot.Accounts.Select(s => RestoreEntity<Account>(s)))
            {
                accounts.Add(account);
            }
        }
    }
}