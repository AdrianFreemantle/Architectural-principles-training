using System;
using Aggregates.Shell.Domain;

namespace Aggregates.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var original = Portfolio.Open(PortfolioId.GenerateId(), AccountType.Cheque, Money.Amount(100));
            original.CreditAccount(AccountType.Cheque, Money.Amount(50));
            var memento = ((IAggregate)original).GetSnapshot();

            var copy = ActivatorHelper.CreateInstanceUsingNonPublicConstructor<Portfolio>(memento.Identity);
            ((IAggregate)copy).RestoreSnapshot(memento);
            copy.CreditAccount(AccountType.Cheque, Money.Amount(5));

            var copyBalance = copy.GetAccountBalance(AccountType.Cheque);
            var originalBalance = original.GetAccountBalance(AccountType.Cheque);

            if (originalBalance == copyBalance)
            {
                throw new Exception("Balances should not match");
            }

            if (original != copy)
            {
                throw new Exception("Aggregates should match by identity");
            }

        }
    }
}

