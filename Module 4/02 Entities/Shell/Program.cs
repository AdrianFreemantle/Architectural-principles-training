using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Shell.Domain;

namespace Shell
{
    class Program
    {
        static void Main(string[] args)
        {
            var id = Guid.NewGuid();
            var ledger1 = new Account(new LedgerId(id));
            var ledger2 = new Account(new LedgerId(id));

            ledger1.Credit(Money.Amount(50));
            ledger2.Credit(Money.Amount(10));

            if (ledger1.Balance() == ledger2.Balance())
            {
                throw new Exception("Balances should not match.");
            }

            if (ledger1 != ledger2)
            {
                throw new Exception("Eqaulity comparison failed");
            }

            if (!ledger1.Equals(ledger2))
            {
                throw new Exception("Eqaulity comparison failed");
            }
        }
    }
}
