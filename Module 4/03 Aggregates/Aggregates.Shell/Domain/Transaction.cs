using System;

namespace Aggregates.Shell.Domain
{
    [Serializable]
    public struct Transaction 
    {
        private readonly Money amount;
        private readonly TransactionType transactionType; 

        public Transaction(Money amount, TransactionType transactionType)
            : this()
        {
            if (transactionType == TransactionType.Unknown)
            {
                throw new ArgumentException("Please specify a debit or credit transaction.");
            }

            if (amount == Money.Zero)
            {
                throw new ArgumentException("A transaction cannot have a zero value.");
            }

            this.transactionType = transactionType;
            this.amount = amount;
        }

        public static implicit operator decimal(Transaction transaction)
        {
            switch (transaction.transactionType)
            {
                case TransactionType.Credit:
                    return transaction.amount;

                case TransactionType.Debit:
                    return -transaction.amount;

                default:
                    return 0;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", transactionType, amount);
        }
    }
}