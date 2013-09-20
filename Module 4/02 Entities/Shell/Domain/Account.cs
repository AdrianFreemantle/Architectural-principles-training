using System.Collections.Generic;
using System.Linq;
using Entities;

namespace Shell.Domain
{
    public class Account : EntityBase
    {
        readonly List<Transaction> transactions;
 
        public Account(LedgerId identity)
            : base(identity)
        {
            transactions = new List<Transaction>();
        }

        public void Debit(Money amount)
        {           
            transactions.Add(new Transaction(amount, TransactionType.Debit));
        }

        public void Credit(Money amount)
        {
            transactions.Add(new Transaction(amount, TransactionType.Credit));
        }

        public decimal Balance()
        {
            return transactions.Sum(transaction => transaction);
        }
    }
}