using System;
using System.Collections.Generic;
using System.Linq;

using EventDriven.Shell.Domain.Events;

namespace EventDriven.Shell.Domain
{
    public partial class Portfolio : Aggregate
    {
        private static readonly Money MinimumPortfolioBalance = Money.Amount(100);

        protected Portfolio(PortfolioId id) 
            : base(id)
        {
            accounts = new HashSet<Account>();
        }

        public static Portfolio Open(PortfolioId id, AccountType accountType, Money initialDeposit)
        {
            if (initialDeposit < MinimumPortfolioBalance)
            {
                throw new InvalidOperationException(String.Format("The intial deposit of {0} is lower than the require a minimum of {1}", initialDeposit, MinimumPortfolioBalance));
            }

            var portfolio = new Portfolio(id);
            portfolio.RaiseEvent(new PortfolioOpened());
            portfolio.OpenAccount(accountType);
            portfolio.CreditAccount(accountType, initialDeposit);

            return portfolio;
        }

        public void ClosePortfolio()
        {
            if (isOpen)
            {
                RaiseEvent(new PortfolioClosed());
            }
        }

        public void OpenAccount(AccountType accountType)
        {
            GaurdPortfolioState();
            RaiseEvent(new AccountOpened(accountType));            
        }

        public void DebitAccount(AccountType accountType, Money debitAmout)
        {
            var currentBalance = GetPortfolioBalance();

            if ((currentBalance - debitAmout) < MinimumPortfolioBalance)
            {
                RaiseEvent(new PortfolioHadInsufficientFundsForDebit(debitAmout, currentBalance, MinimumPortfolioBalance));
                return;
            }

            var account = Get<Account>(new AccountId(accountType));
            account.Debit(debitAmout);
        }

        public void CreditAccount(AccountType accountType, Money creditAmount)
        {
            GaurdPortfolioState();
            var account = Get<Account>(new AccountId(accountType));
            account.Credit(creditAmount);
        }

        public Money GetAccountBalance(AccountType accountType)
        {
            var account = Get<Account>(new AccountId(accountType));
            return account.CurrentBalance();
        }

        public Money GetPortfolioBalance()
        {
            return accounts.Aggregate(Money.Zero, (current, account) => current + account.CurrentBalance());
        }

        private void GaurdPortfolioState()
        {
            if (!isOpen)
            {
                throw new InvalidOperationException("Account is closed");
            }
        }
    }
}