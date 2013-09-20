using System;
using System.Collections.Generic;
using System.Linq;

namespace EventDriven.Shell.Domain
{
    public class OpenedAccount : IDomainEvent
    {
        
    }

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
            GaurdPortfolioState();
            GaurdAccountType(accountType);
            new Account(this, accountType);
        }

        public void DebitAccount(AccountType accountType, Money amount)
        {
            GaurdPortfolioState();

            var account = GetEntity<Account>(new AccountId(accountType));
            account.Debit(amount);
        }

        public void CreditAccount(AccountType accountType, Money amount)
        {
            GaurdPortfolioState();

            var account = GetEntity<Account>(new AccountId(accountType));
            account.Credit(amount);
        }

        public decimal GetAccountBalance(AccountType accountType)
        {
            var account = GetEntity<Account>(new AccountId(accountType));
            return account.Balance();
        }

        private void GaurdPortfolioState()
        {
            if (!State.IsOpen)
            {
                throw new InvalidOperationException("Account is closed");
            }
        }

        private void GaurdAccountType(AccountType accountType)
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
                    var account = new Account(this, new AccountId(entityMemento.Identity.GetId()));
                    ((IEntity)account).RestoreSnapshot(entityMemento);
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