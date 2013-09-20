using System;
using System.Collections.Generic;
using System.Linq;

namespace Aggregates.Shell.Domain
{
    public class Portfolio : TypedAggregate<PortfolioState>
    {
        private static readonly Money MinimumPortfolioBalance = Money.Amount(100);

        public Portfolio(PortfolioId id) 
            : base(id)
        {
            State.IsOpen = true;
        }

        public void ClosePortfolio()
        {
            State.IsOpen = false;
        }

        public void OpenAccount(AccountType accountType)
        {
            GuardPortfolioState();
            GuardAccountType(accountType);

            RegisterOwnedEntity(new Account(accountType));
        }

        public void DebitAccount(AccountType accountType, Money amount)
        {
            GuardPortfolioState();
            GuardPortfolioBalance(accountType, amount);

            var account = GetEntity<Account>(new AccountId(accountType));
            account.Debit(amount);
        }

        public void CreditAccount(AccountType accountType, Money amount)
        {
            GuardPortfolioState();

            var account = GetEntity<Account>(new AccountId(accountType));
            account.Credit(amount);
        }

        public Money GetAccountBalance(AccountType accountType)
        {
            var account = GetEntity<Account>(new AccountId(accountType));
            return account.Balance();
        }

        public Money GetPortfolioBalance()
        {
            Money balance = Money.Zero;

            foreach (var account in Entities.Where(entity => entity is Account))
            {
                balance += ((Account)account).Balance();
            }

            return balance;
        }

        private void GuardPortfolioBalance(AccountType accountType, Money debitAmout)
        {
            var currentBalance = GetPortfolioBalance();

            if ((currentBalance - debitAmout) < MinimumPortfolioBalance)
            {
                throw new InvalidOperationException(String.Format("Unable to withdraw {0} from account {1} on portfolio {2} as it would exceed the portfolio balance limit of {3}", 
                    debitAmout, accountType, this.Identity.GetId(), MinimumPortfolioBalance));
            }
        }

        private void GuardPortfolioState()
        {
            if (!State.IsOpen)
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
                State = State,
                Entities = new HashSet<IMemento>(Entities.Select(entity => entity.GetSnapshot()))
            };
        }

        protected override void RestoreSnapshot(IMemento memento)
        {
            var snapshot = (PortfolioSnapshot)memento;

            State = snapshot.State;

            foreach (var entityMemento in snapshot.Entities)
            {
                var identityType = entityMemento.Identity.GetType();

                if (identityType == typeof (AccountId))
                {
                    var account = new Account(new AccountId(entityMemento.Identity.GetId()));
                    ((IEntity)account).RestoreSnapshot(entityMemento);
                    RegisterOwnedEntity(account);
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Aggregate {0} with Id {1} does not know how to restore memento type {2} with id {3}", 
                        GetType().FullName, Identity, entityMemento.GetType().FullName, entityMemento.Identity));
                }
            }
        }
    }
}