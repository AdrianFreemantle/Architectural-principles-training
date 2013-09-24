using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventDriven.Shell.Domain;

namespace EventDriven.Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Portfolio portfolio = Portfolio.Create(AccountType.Cheque, Money.Amount(100));
                portfolio.CreditAccount(AccountType.Cheque, Money.Amount(10));

                var snapshot = ((IAggregate)portfolio).GetSnapshot();

                IAggregate copy = ActivatorHelper.CreateInstanceUsingNonPublicConstructor<Portfolio>(snapshot.Identity);
                copy.RestoreSnapshot(snapshot);

                IAggregate copy2 = ActivatorHelper.CreateInstanceUsingNonPublicConstructor<Portfolio>(snapshot.Identity);
                copy2.LoadFromHistory(((IAggregate)portfolio).GetUncommittedEvents());

                portfolio.DebitAccount(AccountType.Cheque, Money.Amount(500));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            Console.ReadKey();
        }
    }
}
