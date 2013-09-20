using System;
using ValueObjects.EnumExtensions;
using ValueObjects.Shared;

namespace ValueObjects.ValueTypes.Transaction
{
    public struct Transaction : IEquatable<Transaction>
    {
        private readonly Money amount;
        private readonly TransactionTypeEnum transactionType;
        private readonly DateTime transactionTime;

        public Transaction(TransactionTypeEnum transactionType, Money amount, DateTime transactionTime)
            : this()
        {
            if (transactionTime > DateTime.Now)
            {
                throw new InvalidOperationException("You can't have a future dated transaction");
            }

            if (amount > 0)
            {
                throw new ArgumentException("You cannot perform a transaction for a zero amount.");
            }

            this.transactionType = transactionType;
            this.amount = amount;
            this.transactionTime = transactionTime;
        }

        public Transaction(TransactionTypeEnum transactionType, Money amount)
            : this(transactionType, amount, DateTime.Now)
        {
        }

        public static implicit operator decimal(Transaction transaction)
        {
            switch (transaction.transactionType)
            {
                case TransactionTypeEnum.Deposit:
                    return transaction.amount;
                case TransactionTypeEnum.Withdrawal:
                    return -transaction.amount;
                default:
                    return 0;
            }
        }
      
        public override int GetHashCode()
        {
            return amount.GetHashCode() ^ transactionType.GetHashCode() ^ transactionTime.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Transaction && Equals((Transaction)obj);
        }

        public bool Equals(Transaction other)
        {
            return other.amount == amount
                   && other.transactionType == transactionType
                   && other.transactionTime == transactionTime;
        }

        public static bool operator ==(Transaction left, Transaction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Transaction left, Transaction right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return String.Format("{0} of {1} at {2}", transactionType.GetDescription(), amount, transactionTime);
        }
    }
}