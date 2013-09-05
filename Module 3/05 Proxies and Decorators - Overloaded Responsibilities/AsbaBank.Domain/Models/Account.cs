using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AsbaBank.Domain.Models
{
    public class Account
    {
        [Key]
        public virtual int Id { get; protected set; }
        public virtual string AccountNumber { get; protected set; }
        public virtual bool Closed { get; protected set; }
        public virtual ICollection<Transaction> Ledger { get; set; }
        public virtual ICollection<BankCard> BankCards { get; set; }

        public virtual Client Client { get; protected set; }
        public virtual int ClientId { get; protected set; }

        protected Account()
        {
            //here for entity framework
        }

        protected Account(int clientId)
        {
            if (clientId <= 0)
            {
                throw new ArgumentException("Please provide a valid client Id.");
            }

            AccountNumber = GenerateAccountNumber();
            ClientId = clientId;
            Ledger = new HashSet<Transaction>();
            BankCards = new HashSet<BankCard>();
        }

        public static Account OpenAccount(int clientId)
        {
            return new Account(clientId);
        }

        private static string GenerateAccountNumber()
        {
            var ticks = DateTime.Now.Ticks.ToString();
            return ticks.Substring(ticks.Length - 10);
        }

        public void Debit(decimal amount)
        {
            if (Closed)
            {
                throw new ValidationException("The account is closed");
            }

            var debitAmount = Math.Abs(amount); //make sure the number is positive

            if (GetAccountBalance() < debitAmount)
            {
                throw new ValidationException("Insufficient balance.");
            }

            Ledger.Add(Transaction.DebitTransaction(amount));
        }

        public void Credit(int accountId, decimal amount)
        {
            if (Closed)
            {
                throw new ValidationException("The account is closed");
            }

            Ledger.Add(Transaction.CreditTransaction(amount));
        }

        public decimal GetAccountBalance()
        {
            return Ledger.Sum(ledger => ledger.Amount);
        }        

        public void Close(int accountId)
        {
            if (Closed)
            {
                throw new ValidationException("The account is already closed");
            }

            Closed = true;

            StopBankCard();
        }

        public void IssueBankCard(int accountId)
        {
            if (Closed)
            {
                throw new ValidationException("The account is closed");
            }

            if (BankCards.Any(card => card.AccountId == accountId && !card.Disabled))
            {
                throw new ValidationException("An account may only have one active bank card at a time.");
            }

            BankCards.Add(new BankCard(Id));
        }

        public void StopBankCard()
        {
            BankCard bankCard = BankCards.SingleOrDefault(card => !card.Disabled);

            if (bankCard != null)
            {
                bankCard.Disable();
            }
        }
    }
}