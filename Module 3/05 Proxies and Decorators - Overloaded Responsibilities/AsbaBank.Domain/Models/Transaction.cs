using System;
using System.ComponentModel.DataAnnotations;

namespace AsbaBank.Domain.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; protected set; }
        public decimal Amount { get; protected set; }
        public DateTime Date { get; protected set; }

        protected Transaction()
        {
            //here for entity framework
        }

        protected Transaction(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }

        internal static Transaction CreditTransaction(decimal amount)
        {
            return new Transaction(Math.Abs(amount), DateTime.Now);
        }

        internal static Transaction DebitTransaction(decimal amount)
        {
            return new Transaction(-Math.Abs(amount), DateTime.Now);
        }
    }
}