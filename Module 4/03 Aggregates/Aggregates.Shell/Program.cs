using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Shell.Domain;

namespace Aggregates.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var original = new Portfolio(new PortfolioId("1234567"));

            original.OpenAccount(AccountType.Cheque);
            original.OpenAccount(AccountType.Savings);
            original.CreditAccount(AccountType.Cheque, Money.Amount(50));
            var memento = ((IAggregate)original).GetSnapshot();

            var copy = new Portfolio((PortfolioId)memento.Identity);
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

